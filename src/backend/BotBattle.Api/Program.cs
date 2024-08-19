using System.Text.Json;
using BotBattle.Api.Lobbies;
using BotBattle.Api.Matchmaking;
using BotBattle.Api.Models;
using BotBattle.Api.Options;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services.Map;

var builder = WebApplication.CreateBuilder(args);

var matchmakingOptions = builder.Configuration.GetSection("Matchmaking");
builder.Services.Configure<MatchmakingOptions>(matchmakingOptions);

builder.Services.AddCors();
builder.Services.AddSingleton<Matchmaking>();
builder.Services.AddSingleton(typeof(BroadcastService<>));

builder.Services.AddHostedService<MatchmakingHostedService>();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapGet("/map/generate/{width:int}/{height:int}", (int width, int height) =>
{
    var map = MapGeneratorService.Generate(width, height);

    return Results.Ok(map.Get1DArray());
});

#region Account

app.MapPost("/account/login", (LoginRequest request) =>
{

});

#endregion

#region Matchmaking

app.MapGet("/matchmaking/lobbies", (Matchmaking matchmaking) =>
{
    var lobbies = matchmaking.GetLobbies();

    return Results.Ok(lobbies.Select(lobby => new
    {
        Name = lobby.ProcessId
    }));
});

app.MapPost("/matchmaking/lobbies", (CreateLobbyRequest request, Matchmaking matchmaking) =>
{
    var lobby = matchmaking.CreateNewLobby(request.LobbySize, request.RoundDuration, request.AreaDimensions,
        request.MapTiles);

    return lobby == null
        ? Results.BadRequest()
        : Results.Created($"/matchmaking/lobbies/{lobby.ProcessId}", new { lobby.ProcessId });
});

app.MapGet("/matchmaking/lobbies/{lobbyId:int}", (int lobbyId, Matchmaking matchmaking) =>
{
    var lobby = matchmaking.GetLobbyForId(lobbyId);

    if (lobby == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        name = lobby.ProcessId,
        width = lobby.Width,
        height = lobby.Height,
        players = lobby.Players,
        mapTiles = lobby.MapTiles,
    });
});

app.MapGet("/matchmaking/lobbies/{lobbyId:int}/sse",
    async (int lobbyId, Matchmaking matchmaking, HttpContext httpContext, BroadcastService<BoardState> broadcastService,
        CancellationToken cancellation) =>
    {
        var lobby = matchmaking.GetLobbyForId(lobbyId);

        if (lobby == null)
            return Results.NotFound();

        var response = httpContext.Response;
        response.Headers.Append("Content-Type", "text/event-stream");

        using var subscription = broadcastService.RegisterOutboundChannel();

        while (!cancellation.IsCancellationRequested)
        {
            await foreach (var boardState in subscription.Reader.ReadAllAsync(cancellation))
            {
                await response.WriteAsync(
                    $"data: {JsonSerializer.Serialize(boardState)}{Environment.NewLine}{Environment.NewLine}",
                    cancellation);
                await response.Body.FlushAsync(cancellation);
            }

            break;
        }

        await response.Body.FlushAsync(cancellation);

        return Results.NoContent();
    }
);

#endregion


app.Run();
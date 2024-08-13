using System.Text.Json;
using BotBattle.Api.Matchmaking;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddSingleton(s => new Matchmaking([
        "Dominik",
        "Matthias",
        "Martin",
        "Jonathan",
        "Julien",
        "Stefan",
        "Elias",
        "Dina",
        "Christopher",
        "Hermann"
    ], s.GetRequiredService<BroadcastService<BoardState>>(), s.GetRequiredService<ILogger<Matchmaking>>(),
    s.GetRequiredService<IConfiguration>()));
builder.Services.AddSingleton(typeof(BroadcastService<>));

builder.Services.AddHostedService<MatchmakingHostedService>();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);

// app.UseDefaultFiles();
// app.UseStaticFiles();
app.UseRouting();

app.MapGet("/matchmaking/lobbies", (Matchmaking matchmaking) =>
{
    var lobbies = matchmaking.GetLobbies();

    return Results.Ok(lobbies.Select(l => new
    {
        Name = l.ProcessId
    }));
});

app.MapGet("/matchmaking/lobbies/{lobbyId:int}", (int lobbyId, Matchmaking matchmaking) =>
{
    var lobby = matchmaking.GetLobbyForId(lobbyId);

    if (lobby == null)
        return Results.NotFound();

    var map = MapGeneratorService.Generate(lobby.Width, lobby.Height);
    
    return Results.Ok(new
    {
        name = lobby.ProcessId,
        width = lobby.Width,
        height = lobby.Height,
        players = lobby.Players,
        mapTiles = map.Get1DArray()
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

app.Run();
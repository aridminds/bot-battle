using System.Security.Claims;
using System.Text.Json;
using BotBattle.Api;
using BotBattle.Api.Matchmaking;
using BotBattle.Api.Models;
using BotBattle.Api.Models.Account;
using BotBattle.Api.Options;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services.Map;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var matchmakingOptions = builder.Configuration.GetSection("Matchmaking");
builder.Services.Configure<MatchmakingOptions>(matchmakingOptions);

builder.Services.AddCors();
builder.Services.AddSingleton<Matchmaking>();
builder.Services.AddSingleton(typeof(BroadcastService<>));
builder.Services.AddHostedService<MatchmakingHostedService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.Cookie.Name = "bot-battle-auth"; });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BotBattleSqlite")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:5198")
    .AllowCredentials()
);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("/api");

apiGroup.MapGet("/map/generate/{width:int}/{height:int}", (int width, int height) =>
{
    var map = MapGeneratorService.Generate(width, height);

    return Results.Ok(map.Get1DArray());
}).RequireAuthorization();

#region Account

apiGroup.MapPost("/account/login",
    async (LoginRequest request, UsersDbContext usersDbContext, HttpContext httpContext) =>
    {
        var user = usersDbContext.Users.FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
            return Results.NotFound();

        var passwordMatch = Argon2.Verify(user.PasswordHash, request.Password);

        if (!passwordMatch)
            return Results.Unauthorized();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, "Player"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var redirectUri = request.ReturnUrl ?? httpContext.Request.PathBase + httpContext.Request.Path;

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow,
            RedirectUri = redirectUri
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Results.Ok();
    });

apiGroup.MapPost("/account/register", (LoginRequest request, UsersDbContext usersDbContext) =>
{
    var existingUser = usersDbContext.Users.FirstOrDefault(u => u.Username == request.Username);

    if (existingUser != null)
        return Results.Conflict();

    var passwordHash = Argon2.Hash(request.Password);

    var user = new User(request.Username, passwordHash);

    usersDbContext.Users.Add(user);
    usersDbContext.SaveChanges();

    return Results.Created($"/account/{user.Username}", new { user.Username });
});

apiGroup.MapPost("/account/logout", (HttpContext httpContext) =>
{
    httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Ok();
}).RequireAuthorization();

apiGroup.MapGet("/account/me", (HttpContext httpContext, UsersDbContext usersDbContext) =>
{
    if (httpContext.User.Identity is { IsAuthenticated: false })
    {
        return Results.Unauthorized();
    }

    var user = usersDbContext.Users.FirstOrDefault(u => u.Username == httpContext.User.Identity!.Name);

    if (user == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        user.Username
    });
}).RequireAuthorization();

#endregion

#region Matchmaking

apiGroup.MapGet("/matchmaking/lobbies", (Matchmaking matchmaking) =>
{
    var lobbies = matchmaking.GetLobbies();

    return Results.Ok(lobbies.Select(lobby => new
    {
        Name = lobby.ProcessId
    }));
});

apiGroup.MapPost("/matchmaking/lobbies", (CreateLobbyRequest request, Matchmaking matchmaking) =>
{
    var lobby = matchmaking.CreateNewLobby(request.LobbySize, request.RoundDuration, request.AreaDimensions,
        request.MapTiles);

    return lobby == null
        ? Results.BadRequest()
        : Results.Created($"/matchmaking/lobbies/{lobby.ProcessId}", new { lobby.ProcessId });
}).RequireAuthorization();

apiGroup.MapGet("/matchmaking/lobbies/{lobbyId:int}", (int lobbyId, Matchmaking matchmaking) =>
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

apiGroup.MapGet("/matchmaking/lobbies/{lobbyId:int}/sse",
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
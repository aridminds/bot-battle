using System.Collections.Concurrent;
using BotBattle.Api.Lobbies;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;

namespace BotBattle.Api.Matchmaking;

public class Matchmaking
{
    private readonly int[] _arenaDimensions;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentBag<LobbyProcess> _currentLobbies = [];
    private readonly BroadcastService<BoardState> _broadcastService;

    private readonly ILogger<Matchmaking> _logger;
    private readonly int _maximumConcurrentLobbies;

    private readonly string _pathToLobbyServerExecutable;
    private ConcurrentStack<string> _availablePlayers;

    public Matchmaking(HashSet<string> availablePlayers, BroadcastService<BoardState> broadcastService, ILogger<Matchmaking> logger, IConfiguration configuration)
    {
        _broadcastService = broadcastService;
        _logger = logger;
        _configuration = configuration;
        _availablePlayers = new ConcurrentStack<string>(availablePlayers);

        _pathToLobbyServerExecutable = configuration.GetValue<string>("PathToLobbyServerExecutable");
        _maximumConcurrentLobbies = configuration.GetValue<int>("MaximumConcurrentLobbies");
        _arenaDimensions = configuration.GetValue<int[]>("ArenaDimensions") ?? [48, 27];
    }

    public Task QueueNewLobbies(CancellationToken cancellationToken)
    {
        if (_currentLobbies.Count >= _maximumConcurrentLobbies)
            return Task.CompletedTask;

        var newPlayerPair = GetNewPlayerPair();

        if (newPlayerPair.Length == 0) return Task.CompletedTask;

        var newLobby = CreateNewLobby(newPlayerPair, cancellationToken);

        _logger.LogInformation("New lobby created: {lobbyId}", newLobby.ProcessId.ToString());

        _currentLobbies.Add(newLobby);

        return Task.CompletedTask;
    }

    public LobbyProcess[] GetLobbies()
    {
        return _currentLobbies.ToArray();
    }

    public LobbyProcess? GetLobbyForId(int id)
    {
        return _currentLobbies.FirstOrDefault(l => l.ProcessId == id);
    }

    private LobbyProcess CreateNewLobby(string[] playerNames, CancellationToken cancellationToken)
    {
        var newLobby = new LobbyProcess(playerNames, _arenaDimensions, _pathToLobbyServerExecutable, cancellationToken);
        newLobby.Start(_broadcastService);
        return newLobby;
    }

    private string[] GetNewPlayerPair()
    {
        var random = new Random();

        var newPlayersArray = _availablePlayers.ToArray();
        random.Shuffle(newPlayersArray);
        _availablePlayers = new ConcurrentStack<string>(newPlayersArray);

        var players = new string[10];
        var count = _availablePlayers.TryPopRange(players);

        return count == 0 ? [] : players;
    }
}
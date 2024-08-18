using System.Collections.Concurrent;
using BotBattle.Api.Lobbies;
using BotBattle.Api.Options;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using Microsoft.Extensions.Options;

namespace BotBattle.Api.Matchmaking;

public class Matchmaking
{
    private readonly int[] _arenaDimensions;
    private readonly BroadcastService<BoardState> _broadcastService;
    private readonly ConcurrentBag<LobbyProcess?> _currentLobbies = [];

    private readonly ILogger<Matchmaking> _logger;
    private readonly int _maximumConcurrentLobbies;

    private readonly string _pathToLobbyServerExecutable;
    private ConcurrentStack<string> _availablePlayers;
    private int _roundDuration;

    private CancellationTokenSource _cancellationTokenSource;

    public Matchmaking(BroadcastService<BoardState> broadcastService, IOptions<MatchmakingOptions> matchmakingOptions,
        ILogger<Matchmaking> logger)
    {
        _broadcastService = broadcastService;
        _logger = logger;
        _availablePlayers = new ConcurrentStack<string>(matchmakingOptions.Value.AvailablePlayers);
        _pathToLobbyServerExecutable = matchmakingOptions.Value.PathToLobbyServerExecutable;
        _maximumConcurrentLobbies = matchmakingOptions.Value.MaximumConcurrentLobbies;
        _arenaDimensions = matchmakingOptions.Value.ArenaDimensions;
        _roundDuration = matchmakingOptions.Value.RoundDuration;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public Task QueueNewLobbies()
    {
        if (_currentLobbies.Count >= _maximumConcurrentLobbies)
            return Task.CompletedTask;

        var newPlayerPair = GetNewPlayerPair();

        if (newPlayerPair.Length == 0) return Task.CompletedTask;

        var newLobby = CreateNewLobby(2, _roundDuration, _arenaDimensions, []);

        _currentLobbies.Add(newLobby);

        return Task.CompletedTask;
    }

    public LobbyProcess?[] GetLobbies()
    {
        return _currentLobbies.ToArray();
    }

    public LobbyProcess? GetLobbyForId(int id)
    {
        return _currentLobbies.FirstOrDefault(l => l.ProcessId == id);
    }

    public LobbyProcess? CreateNewLobby(int lobbySize, int roundDuration, int[] areaDimensions, int[] mapTiles)
    {
        var newPlayerPair = GetNewPlayerPair(lobbySize);

        if (newPlayerPair.Length == 0) return null;

        var newLobby = new LobbyProcess(newPlayerPair, areaDimensions, mapTiles, roundDuration,
            _pathToLobbyServerExecutable
            , _cancellationTokenSource.Token);
        
        newLobby.Start(_broadcastService);
        _currentLobbies.Add(newLobby);

        return newLobby;
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private string[] GetNewPlayerPair(int lobbySize = 2)
    {
        var random = new Random();

        var newPlayersArray = _availablePlayers.ToArray();
        random.Shuffle(newPlayersArray);
        _availablePlayers = new ConcurrentStack<string>(newPlayersArray);

        var players = new string[lobbySize];
        var count = _availablePlayers.TryPopRange(players);

        return count == 0 ? [] : players;
    }
}
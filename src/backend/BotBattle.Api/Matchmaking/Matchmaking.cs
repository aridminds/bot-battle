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
    private readonly ConcurrentBag<LobbyProcess> _currentLobbies = [];

    private readonly ILogger<Matchmaking> _logger;
    private readonly int _maximumConcurrentLobbies;

    private readonly string _pathToLobbyServerExecutable;
    private ConcurrentStack<string> _availablePlayers;
    private int _roundDuration;

    public Matchmaking(BroadcastService<BoardState> broadcastService,IOptions<MatchmakingOptions> matchmakingOptions, ILogger<Matchmaking> logger)
    {
        _broadcastService = broadcastService;
        _logger = logger;
        _availablePlayers = new ConcurrentStack<string>(matchmakingOptions.Value.AvailablePlayers);

        _pathToLobbyServerExecutable = matchmakingOptions.Value.PathToLobbyServerExecutable;
        _maximumConcurrentLobbies = matchmakingOptions.Value.MaximumConcurrentLobbies;
        _arenaDimensions = matchmakingOptions.Value.ArenaDimensions;
        _roundDuration = matchmakingOptions.Value.RoundDuration;
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
        var newLobby = new LobbyProcess(playerNames, _arenaDimensions, _pathToLobbyServerExecutable, cancellationToken, _roundDuration);
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
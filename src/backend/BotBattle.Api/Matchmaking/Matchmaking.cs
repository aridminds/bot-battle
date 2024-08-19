using System.Collections.Concurrent;
using BotBattle.Api.Lobbies;
using BotBattle.Api.Options;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using Microsoft.Extensions.Options;

namespace BotBattle.Api.Matchmaking;

public class Matchmaking
{
    private readonly ConcurrentDictionary<string, LobbyProcess> _currentLobbies = [];

    private readonly BroadcastService<BoardState> _broadcastService;
    private ConcurrentStack<string> _availablePlayers;
    private readonly string _pathToLobbyServerExecutable;
    private readonly int _maximumConcurrentLobbies;
    private readonly int[] _arenaDimensions;
    private readonly int _roundDuration;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public Matchmaking(BroadcastService<BoardState> broadcastService, IOptions<MatchmakingOptions> matchmakingOptions)
    {
        _broadcastService = broadcastService;
        _availablePlayers = new ConcurrentStack<string>(matchmakingOptions.Value.AvailablePlayers);
        _pathToLobbyServerExecutable = matchmakingOptions.Value.PathToLobbyServerExecutable;
        _maximumConcurrentLobbies = matchmakingOptions.Value.MaximumConcurrentLobbies;
        _arenaDimensions = matchmakingOptions.Value.ArenaDimensions;
        _roundDuration = matchmakingOptions.Value.RoundDuration;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public LobbyProcess[] GetLobbies()
    {
        return _currentLobbies.Values.ToArray();
    }

    public LobbyProcess? GetLobbyForId(int id)
    {
        return _currentLobbies.GetValueOrDefault(id.ToString());
    }

    public LobbyProcess? CreateNewLobby(int lobbySize, int roundDuration, int[] areaDimensions, int[] mapTiles)
    {
        var newPlayerPair = GetNewPlayerPair(lobbySize);
        if (newPlayerPair.Length == 0) return null;

        var newLobby = new LobbyProcess(newPlayerPair, areaDimensions, mapTiles, roundDuration,
            _pathToLobbyServerExecutable,
            _cancellationTokenSource.Token);

        newLobby.Start(_broadcastService);
        _currentLobbies.TryAdd(newLobby.ProcessId.ToString(), newLobby);

        newLobby.LobbyFinished += OnLobbyFinished;
        return newLobby;
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private void OnLobbyFinished(object? sender, LobbyProcess lobby)
    {
        _availablePlayers.PushRange(lobby.Players);
        _currentLobbies.TryRemove(lobby.ProcessId.ToString(), out _);
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
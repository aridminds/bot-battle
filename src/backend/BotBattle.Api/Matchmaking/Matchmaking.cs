using System.Collections.Concurrent;
using System.Text.Json;
using BotBattle.Api.Models.LobbySpawner;
using BotBattle.Api.Options;
using BotBattle.Api.Services.LobbySpawner;
using BotBattle.Engine.Models.Lobby;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BotBattle.Api.Matchmaking;

public class Matchmaking
{
    private readonly ConcurrentDictionary<Guid, Lobby> _currentLobbies = [];
    private ConcurrentStack<string> _availablePlayers;
    private readonly string _pathToLobbyServerExecutable;
    private readonly int _maximumConcurrentLobbies;
    private readonly int[] _arenaDimensions;
    private readonly int _roundDuration;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILobbySpawner<DaemonLobbyOptions> _daemonLobbySpawner;

    public Matchmaking(IOptions<MatchmakingOptions> matchmakingOptions, IConnectionMultiplexer connectionMultiplexer,
        ILobbySpawner<DaemonLobbyOptions> lobbySpawner)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _availablePlayers = new ConcurrentStack<string>(matchmakingOptions.Value.AvailablePlayers);
        _pathToLobbyServerExecutable = matchmakingOptions.Value.PathToLobbyServerExecutable;
        _maximumConcurrentLobbies = matchmakingOptions.Value.MaximumConcurrentLobbies;
        _arenaDimensions = matchmakingOptions.Value.ArenaDimensions;
        _roundDuration = matchmakingOptions.Value.RoundDuration;
        _cancellationTokenSource = new CancellationTokenSource();
        _daemonLobbySpawner = lobbySpawner;
    }

    public Lobby[] GetLobbies()
    {
        return _currentLobbies.Values.ToArray();
    }

    public Lobby? GetLobbyForId(Guid id)
    {
        return _currentLobbies.GetValueOrDefault(id);
    }

    public Lobby? CreateNewLobby(int lobbySize, int roundDuration, int[] areaDimensions, int[] mapTiles)
    {
        var newPlayerPair = GetNewPlayerPair(lobbySize);
        if (newPlayerPair.Length == 0) return null;

        var database = _connectionMultiplexer.GetDatabase();

        var lobbyId = Guid.NewGuid();
        var lobbyKey = $"lobby:{lobbyId}";

        database.StringSet(lobbyKey, JsonSerializer.Serialize(new LobbyOptions
        {
            Players = newPlayerPair,
            ArenaDimension = areaDimensions,
            RoundDuration = roundDuration,
            MapTiles = mapTiles,
        }));

        var newLobby = _daemonLobbySpawner.Spawn(new DaemonLobbyOptions
        {
            LobbyId = lobbyId,
            PathToLobbyServerExecutable = _pathToLobbyServerExecutable,
            Players = newPlayerPair,
            ArenaDimension = areaDimensions,
            MapTiles = mapTiles,
            RoundDuration = roundDuration,
        }, _cancellationTokenSource.Token).Result;

        _currentLobbies.TryAdd(newLobby.LobbyId, newLobby);

        newLobby.LobbyFinished += OnLobbyFinished;
        return newLobby;
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private void OnLobbyFinished(object? sender, Lobby lobby)
    {
        _availablePlayers.PushRange(lobby.Players);
        _currentLobbies.TryRemove(lobby.LobbyId, out _);
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
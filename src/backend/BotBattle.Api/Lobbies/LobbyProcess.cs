using System.Text.Json;
using System.Threading.Channels;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using CliWrap;

namespace BotBattle.Api.Lobbies;

public class LobbyProcess
{
    private readonly int[] _arenaDimension;
    private readonly int[] _mapTiles;
    private readonly string[] _players;
    private readonly string _pathToLobbyServerExecutable;
    private readonly int _roundDuration;
    private readonly Channel<BoardState> _boardStateChannel = Channel.CreateUnbounded<BoardState>();
    private readonly CancellationToken _cancellationToken;

    public LobbyProcess(string[] playerNames, int[] arenaDimension, int[] mapTiles, int roundDuration,
        string pathToLobbyServerExecutable,
        CancellationToken cancellationToken)
    {
        _players = playerNames;
        _arenaDimension = arenaDimension;
        _mapTiles = mapTiles;
        _pathToLobbyServerExecutable = pathToLobbyServerExecutable;
        _roundDuration = roundDuration;
        _cancellationToken = cancellationToken;
    }

    public int ProcessId { get; private set; }
    public int Width => _arenaDimension[0];
    public int Height => _arenaDimension[1];
    public string[] Players => _players;
    public int[] ArenaDimension => _arenaDimension;
    public int[] MapTiles => _mapTiles;

    public void Start(BroadcastService<BoardState> broadcast)
    {
        Task.Factory.StartNew(b => RunAsync((BroadcastService<BoardState>)b!), broadcast, _cancellationToken);
    }

    private async void RunAsync(BroadcastService<BoardState> broadcast)
    {
        _ = broadcast.Broadcast(_boardStateChannel.Reader, _cancellationToken);

        var task = Cli
                       .Wrap(_pathToLobbyServerExecutable)
                       .WithArguments([
                           string.Join(',', _players),
                           string.Join(',', _arenaDimension),
                           _roundDuration.ToString(),
                           string.Join(',', _mapTiles)
                       ]) |
                   (async stdOutput =>
                   {
                       var boardState = JsonSerializer.Deserialize<BoardState>(stdOutput);

                       if (boardState == null)
                           return;

                       await _boardStateChannel.Writer.WriteAsync(boardState, _cancellationToken);
                   });

        var process = task.ExecuteAsync(_cancellationToken);
        ProcessId = process.ProcessId;

        try
        {
            await process;
        }
        catch (OperationCanceledException e)
        {
            // Console.WriteLine(e);
        }
    }
}
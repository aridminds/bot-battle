using System.Text.Json;
using System.Threading.Channels;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using CliWrap;

namespace BotBattle.Api.Lobbies;

public class LobbyProcess
{
    private readonly Channel<BoardState> _boardStateChannel = Channel.CreateUnbounded<BoardState>();
    private readonly CancellationToken _cancellationToken;

    private readonly string _pathToLobbyServerExecutable;
    private readonly int _roundDuration;

    public LobbyProcess(string[] playerNames, int[] arenaDimension, int[] mapTiles, int roundDuration,
        string pathToLobbyServerExecutable, CancellationToken cancellationToken)
    {
        Players = playerNames;
        ArenaDimension = arenaDimension;
        MapTiles = mapTiles;
        _pathToLobbyServerExecutable = pathToLobbyServerExecutable;
        _roundDuration = roundDuration;
        _cancellationToken = cancellationToken;
    }

    public int ProcessId { get; private set; }
    public int Width => ArenaDimension[0];
    public int Height => ArenaDimension[1];
    public string[] Players { get; }
    public int[] ArenaDimension { get; }
    public int[] MapTiles { get; }

    public event EventHandler<LobbyProcess> LobbyFinished;

    public void Start(BroadcastService<BoardState> broadcast)
    {
        var task = Cli
                       .Wrap(_pathToLobbyServerExecutable)
                       .WithArguments([
                           string.Join(',', Players),
                           string.Join(',', ArenaDimension),
                           _roundDuration.ToString(),
                           string.Join(',', MapTiles)
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

        Task.Factory.StartNew(
            b => RunAsync(((BroadcastService<BoardState>, CommandTask<CommandResult>))b!),
            (broadcast, process), _cancellationToken);
    }

    private async void RunAsync((BroadcastService<BoardState>, CommandTask<CommandResult>) tupleOfServices)
    {
        _ = tupleOfServices.Item1.Broadcast(_boardStateChannel.Reader, _cancellationToken);

        try
        {
            await tupleOfServices.Item2;
        }
        catch (OperationCanceledException e)
        {
        }
        finally
        {
            LobbyFinished?.Invoke(this, this);
        }
    }
}
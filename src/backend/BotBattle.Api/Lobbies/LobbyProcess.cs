using System.Text.Json;
using System.Threading.Channels;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using CliWrap;

namespace BotBattle.Api.Lobbies;

public class LobbyProcess
{
    private readonly int[] _arenaDimension;
    private readonly CancellationToken _cancellationToken;
    private readonly string _pathToLobbyServerExecutable;
    private readonly Channel<BoardState> BoardStateChannel = Channel.CreateUnbounded<BoardState>();

    public LobbyProcess(string[] playerNames, int[] arenaDimension, string pathToLobbyServerExecutable,
        CancellationToken cancellationToken)
    {
        Players = playerNames;
        _arenaDimension = arenaDimension;
        _pathToLobbyServerExecutable = pathToLobbyServerExecutable;


        _cancellationToken = cancellationToken;
    }

    public int ProcessId { get; set; }
    public int Width => _arenaDimension[0];
    public int Height => _arenaDimension[1];
    public string[] Players { get; }

    public void Start(BroadcastService<BoardState> broadcast)
    {
        Task.Factory.StartNew(b => RunAsync((BroadcastService<BoardState>)b!), broadcast);
    }

    private async void RunAsync(BroadcastService<BoardState> broadcast)
    {
        _ = broadcast.Broadcast(BoardStateChannel.Reader, _cancellationToken);
        var task = Cli
            .Wrap(_pathToLobbyServerExecutable)
            .WithArguments([string.Join(',', Players), string.Join(',', _arenaDimension)]) | (async stdOutput =>
        {
            var boardState = JsonSerializer.Deserialize<BoardState>(stdOutput);

            if (boardState == null)
                return;

            await BoardStateChannel.Writer.WriteAsync(boardState, _cancellationToken);
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
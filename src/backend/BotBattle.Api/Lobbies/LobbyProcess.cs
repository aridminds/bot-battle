using System.Text.Json;
using System.Threading.Channels;
using BotBattle.Api.Services;
using BotBattle.Engine.Models;
using CliWrap;

namespace BotBattle.Api.Lobbies;

public class LobbyProcess
{
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

    public Guid LobbyId { get; private set; } = Guid.NewGuid();
    public int Width => ArenaDimension[0];
    public int Height => ArenaDimension[1];
    public string[] Players { get; }
    public int[] ArenaDimension { get; }
    public int[] MapTiles { get; }

    public event EventHandler<LobbyProcess> LobbyFinished;

    public void Start()
    {
        var task = Cli
            .Wrap(_pathToLobbyServerExecutable)
            .WithArguments([
                LobbyId.ToString()
            ]);

        var process = task.ExecuteAsync(_cancellationToken);

        Task.Factory.StartNew(b => RunAsync((CommandTask<CommandResult>)b!), process, _cancellationToken);
    }

    private async void RunAsync(CommandTask<CommandResult> commandTask)
    {
        try
        {
            await commandTask;
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
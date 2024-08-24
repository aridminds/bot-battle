using BotBattle.Api.Models.LobbySpawner;
using CliWrap;

namespace BotBattle.Api.Services.LobbySpawner;

public class DaemonLobbySpawner : ILobbySpawner<DaemonLobbyOptions>
{
    public Task<Lobby> Spawn(DaemonLobbyOptions options, CancellationToken cancellationToken)
    {
        var newLobby = new Lobby
        {
            LobbyId = options.LobbyId,
            Players = options.Players,
            ArenaDimension = options.ArenaDimension,
            MapTiles = options.MapTiles,
            Width = options.ArenaDimension[0],
            Height = options.ArenaDimension[1]
        };

        var task = Cli
            .Wrap(options.PathToLobbyServerExecutable)
            .WithArguments([
                newLobby.LobbyId.ToString()
            ]);

        var process = task.ExecuteAsync(cancellationToken);

        Task.Factory.StartNew(b => RunAsync(((CommandTask<CommandResult>, Lobby))b!), (process, newLobby),
            cancellationToken);

        return Task.FromResult(newLobby);
    }

    private static async void RunAsync((CommandTask<CommandResult> commandTask, Lobby lobby) param)
    {
        try
        {
            await param.commandTask;
        }
        catch (OperationCanceledException e)
        {
            // ignored
        }
        finally
        {
            param.lobby.OnLobbyFinished();
        }
    }
}
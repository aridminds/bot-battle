// using System.Text.Json;
// using BotBattle.Api.Models.LobbySpawner;
// using BotBattle.Engine.Models;
// using CliWrap;
//
// namespace BotBattle.Api.Services.LobbySpawner;
//
// public class DaemonLobbySpawner : ILobbySpawner<DaemonLobbyOptions>
// {
//     public Task<Lobby> Spawn(DaemonLobbyOptions options)
//     {
//         var newLobby = new Lobby();
//
//         var task = Cli
//                        .Wrap(options.PathToLobbyServerExecutable)
//                        .WithArguments([
//                            string.Join(',', options.Players),
//                            string.Join(',', options.ArenaDimension),
//                            options.RoundDuration.ToString(),
//                            string.Join(',', options.MapTiles)
//                        ]) |
//                    (async stdOutput =>
//                    {
//                        var boardState = JsonSerializer.Deserialize<BoardState>(stdOutput);
//
//                        if (boardState == null)
//                            return;
//
//                        await newLobby.BoardStateChannel.Writer.WriteAsync(boardState);
//                    });
//
//         var process = task.ExecuteAsync();
//         var processId = process.ProcessId;
//
//         Task.Factory.StartNew(
//             b => RunAsync(((BroadcastService<BoardState>, CommandTask<CommandResult>))b!),
//             (broadcast, process));
//
//         return Task.FromResult(new Lobby());
//     }
//     
//     private async void RunAsync((BroadcastService<BoardState>, CommandTask<CommandResult>) tupleOfServices)
//     {
//         _ = tupleOfServices.Item1.Broadcast(_boardStateChannel.Reader, _cancellationToken);
//
//         try
//         {
//             await tupleOfServices.Item2;
//         }
//         catch (OperationCanceledException e)
//         {
//         }
//         finally
//         {
//             LobbyFinished?.Invoke(this, this);
//         }
//     }
// }
using System.Text.Json;
using BotBattle.LobbyServer;

var playerNames = args[0].Split(',');
var arenaDimensions = args[1].Split(',');
var roundDuration = int.Parse(args[2]);

var newLobby = new Lobby(playerNames, int.Parse(arenaDimensions[0]), int.Parse(arenaDimensions[1]), roundDuration);

var cancellationTokenSource = new CancellationTokenSource();

// _ = Task.Run(async () =>
// {
//     TextReader reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
//     var readTask = reader.ReadLineAsync();
//     
//     while (await readTask != null)
//     {
//         //Read from the stdin to get the command from the api
//         var input = await readTask;
//         readTask = reader.ReadLineAsync();
//     }
// }, cancellationTokenSource.Token);

await newLobby.Run(state =>
{
    //Write to the stdout to communicate back to the api
    Console.WriteLine(JsonSerializer.Serialize(state));
}, cancellationTokenSource.Token);

//Return the winner id
return 0;
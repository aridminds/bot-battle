using System.Text.Json;
using BotBattle.Engine.Models.Lobby;
using BotBattle.LobbyServer;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

var lobbyId = args[0];

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { "localhost:6379" },
    AbortOnConnectFail = false,
    ConnectRetry = 5,
}));

var serviceProvider = serviceCollection.BuildServiceProvider();
var scope = serviceProvider.CreateScope();

var connectionMultiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
var database = connectionMultiplexer.GetDatabase();

var lobbyKey = $"lobby:{lobbyId}";

if (!database.KeyExists(lobbyKey))
{
    throw new Exception("Lobby does not exist");
}

var lobbyData = database.StringGet(lobbyKey);

if (lobbyData.IsNullOrEmpty)
{
    throw new Exception("Lobby data is empty");
}

var lobbyOptions = JsonSerializer.Deserialize<LobbyOptions>(lobbyData);

if (lobbyOptions == null)
{
    throw new Exception("Failed to deserialize lobby data");
}

var newLobby = new Lobby(lobbyOptions.Players,
    lobbyOptions.ArenaDimension[0],
    lobbyOptions.ArenaDimension[1],
    lobbyOptions.RoundDuration);

var cancellationTokenSource = new CancellationTokenSource();
var subscriber = connectionMultiplexer.GetSubscriber();

await newLobby.Run((state) => { subscriber.Publish(lobbyId, JsonSerializer.Serialize(state)); },
    cancellationTokenSource.Token);
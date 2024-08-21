using System.Threading.Channels;
using BotBattle.Engine.Models;

namespace BotBattle.Api.Models.LobbySpawner;

public class Lobby
{
    public readonly Channel<BoardState> BoardStateChannel = Channel.CreateUnbounded<BoardState>();
    private readonly CancellationToken _cancellationToken;
}
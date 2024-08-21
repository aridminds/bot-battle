using BotBattle.Api.Models;
using BotBattle.Api.Models.LobbySpawner;

namespace BotBattle.Api.Services.LobbySpawner;

public interface ILobbySpawner<T> where T : ILobbyOptions
{
    Task<Lobby> Spawn(T options);
}
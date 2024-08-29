using BotBattle.Core;

namespace BotBattle.Api.Models.LobbySpawner;

public interface ILobbyOptions
{
    Player[] Players { get; }
    int[] ArenaDimension { get; }
    int[] MapTiles { get; }
    int RoundDuration { get; }
}
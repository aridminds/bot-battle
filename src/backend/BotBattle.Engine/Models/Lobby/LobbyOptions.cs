using BotBattle.Core;

namespace BotBattle.Engine.Models.Lobby;

public class LobbyOptions
{
    public Player[] Players { get; set; }
    public int[] ArenaDimension { get; set; }
    public int[] MapTiles { get; set; }
    public int RoundDuration { get; set; }
}
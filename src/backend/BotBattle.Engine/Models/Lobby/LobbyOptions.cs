namespace BotBattle.Engine.Models.Lobby;

public class LobbyOptions
{
    public string[] Players { get; set; }
    public int[] ArenaDimension { get;set; }
    public int[] MapTiles { get; set;}
    public int RoundDuration { get; set;}
}
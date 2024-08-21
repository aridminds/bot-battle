namespace BotBattle.Api.Models.LobbySpawner;

public class DaemonLobbyOptions : ILobbyOptions
{
    public string PathToLobbyServerExecutable { get; set; }
    public string[] Players { get; }
    public int[] ArenaDimension { get; }
    public int[] MapTiles { get; }
    public int RoundDuration { get; }
}
namespace BotBattle.Api.Models.LobbySpawner;

public class DaemonLobbyOptions : ILobbyOptions
{
    public Guid LobbyId { get; set; }
    public string PathToLobbyServerExecutable { get; set; }
    public string[] Players { get; set; }
    public int[] ArenaDimension { get; set; }
    public int[] MapTiles { get; set; }
    public int RoundDuration { get; set; }
}
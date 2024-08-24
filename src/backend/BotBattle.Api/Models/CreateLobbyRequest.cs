namespace BotBattle.Api.Models;

public class CreateLobbyRequest
{
    public int LobbySize { get; set; }
    public int RoundDuration { get; set; }
    public int[] AreaDimensions { get; set; }
    public int[] MapTiles { get; set; }
}
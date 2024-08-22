namespace BotBattle.Api.Models.LobbySpawner;

public interface ILobbyOptions
{
    string[] Players { get; }
    int[] ArenaDimension { get; }
    int[] MapTiles { get; }
    int RoundDuration { get; }
}
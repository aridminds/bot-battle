namespace BotBattle.Api.Options;

public class MatchmakingOptions
{
    public string PathToLobbyServerExecutable { get; set; }
    public int MaximumConcurrentLobbies { get; set; }
    public int[] ArenaDimensions { get; set; }
    public int RoundDuration { get; set; }
    public string[] AvailablePlayers { get; set; }
}
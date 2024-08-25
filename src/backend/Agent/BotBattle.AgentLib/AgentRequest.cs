namespace BotBattle.AgentLib;

public class AgentRequest
{
    public Arena Arena { get; set; }
    public Tank MyTank { get; set; }
    public byte[] Hash { get; set; }
}
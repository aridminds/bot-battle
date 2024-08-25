namespace BotBattle.AgentLib;

public interface IAgent
{
    AgentResponse CalculateAction(AgentRequest request);
}
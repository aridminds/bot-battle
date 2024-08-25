using BotBattle.AgentLib;

namespace BotBattle.Agent;

public class Agent : IAgent
{
    public AgentResponse CalculateAction(AgentRequest request)
    {
        return new AgentResponse
        {
            Action = new Drive
            {
                Id = Guid.NewGuid()
            }
        };
    }
}
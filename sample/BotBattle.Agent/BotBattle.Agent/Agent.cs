using BotBattle.AgentLib;
using BotBattle.AgentLib.Enums;
using Action = BotBattle.AgentLib.Action;

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
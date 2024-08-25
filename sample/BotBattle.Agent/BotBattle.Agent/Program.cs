using System.Runtime.InteropServices;
using Extism;

namespace BotBattle.Agent;

public static class Program
{
    private static readonly Agent Agent = new();

    public static void Main()
    {
        // This method is required for compilation
    }

    [UnmanagedCallersOnly(EntryPoint = "calculate_action")]
    public static int CalculateAction()
    {
        var parameters = Pdk.GetInputJson(AgentJsonContext.Default.AgentRequest);

        if (parameters == null)
        {
            Pdk.SetError("Failed to parse input JSON");
            return 1;
        }

        var action = Agent.CalculateAction(parameters);
        Pdk.SetOutputJson(action, AgentJsonContext.Default.AgentResponse);
        return 0;
    }
}
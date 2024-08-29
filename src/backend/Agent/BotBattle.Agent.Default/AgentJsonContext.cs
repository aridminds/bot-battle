using System.Text.Json.Serialization;
using BotBattle.AgentLib;

namespace BotBattle.Agent.Default;

[JsonSerializable(typeof(AgentRequest))]
[JsonSerializable(typeof(AgentResponse))]
public partial class AgentJsonContext : JsonSerializerContext
{
}
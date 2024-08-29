using System.Text.Json.Serialization;
using BotBattle.AgentLib;

namespace BotBattle.Agent;

[JsonSerializable(typeof(AgentRequest))]
[JsonSerializable(typeof(AgentResponse))]
public partial class AgentJsonContext : JsonSerializerContext
{
}
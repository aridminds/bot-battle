using System.Text.Json.Serialization;

namespace BotBattle.AgentLib;

[JsonDerivedType(derivedType: typeof(Shoot), typeDiscriminator: "Shoot")]
[JsonDerivedType(derivedType: typeof(Rotate), typeDiscriminator: "Rotate")]
[JsonDerivedType(derivedType: typeof(Drive), typeDiscriminator: "Drive")]
public class Action
{
    public Guid Id { get; set; }
}
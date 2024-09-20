using System.Text.Json.Serialization;

namespace BotBattle.AgentLib;

[JsonDerivedType(derivedType: typeof(Shoot), typeDiscriminator: "Shoot")]
[JsonDerivedType(derivedType: typeof(Rotate), typeDiscriminator: "Rotate")]
[JsonDerivedType(derivedType: typeof(Drive), typeDiscriminator: "Drive")]
[JsonDerivedType(derivedType: typeof(UseItem), typeDiscriminator: "UseItem")]
public class Action
{
    public Guid Id { get; set; }
}
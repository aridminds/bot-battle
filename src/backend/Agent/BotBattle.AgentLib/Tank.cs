using BotBattle.Core.Enums;

namespace BotBattle.AgentLib;

public class Tank : Entity
{
    public string Name { get; set; }
    public int Health { get; set; }
    public WeaponSystem WeaponSystem { get; set; }
    public List<CollectibleItemType> Inventory { get; set; } = [];
}
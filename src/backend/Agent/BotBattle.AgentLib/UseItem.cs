using BotBattle.Core.Enums;

namespace BotBattle.AgentLib;

public class UseItem: Action
{
    public CollectibleItemType ItemType { get; set; }
}
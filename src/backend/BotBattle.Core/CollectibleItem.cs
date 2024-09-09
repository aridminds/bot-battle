using BotBattle.Core.Enums;

namespace BotBattle.Core;

public class CollectibleItem
{
    public Guid Id { get; set; }
    public required Position Position { get; set; }
    public required CollectibleItemType Type { get; set; }
}
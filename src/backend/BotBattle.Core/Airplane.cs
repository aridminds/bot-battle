using BotBattle.Core.Enums;

namespace BotBattle.Core;

public class Airplane
{
    public bool IsFlying { get; set; } 
    public Position Position { get; set; }
    public ParachuteStatus ParachuteStatus { get; set; } = ParachuteStatus.Delivered;
    public Position DroppingGiftPosition { get; set; }
    public CollectibleItemType DroppingGiftType { get; set; }
}
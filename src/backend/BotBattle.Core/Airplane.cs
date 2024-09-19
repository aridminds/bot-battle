using System.Security.Cryptography.X509Certificates;
using BotBattle.Core.Enums;

namespace BotBattle.Core;

public class Airplane
{
    public bool IsFlying { get; set; } 
    public Position Position { get; set; } = new(0,0, Direction.East);
    public ParachuteStatus ParachuteStatus { get; set; } = ParachuteStatus.Delivered;
    public Position DroppingGiftPosition { get; set; } = new(0,0);
    public CollectibleItemType DroppingGiftType { get; set; }
}
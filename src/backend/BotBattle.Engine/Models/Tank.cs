using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;
using MessagePack;

namespace BotBattle.Engine.Models;

[MessagePackObject]
public class Tank
{
    [Key(0)]
    public required string Name { get; init; }
    
    [Key(1)]
    public Position Position { get; set; } = new(0, 0);
    
    [Key(2)]
    public int Health { get; set; } = 1000;
    
    [Key(3)]
    public TankStatus Status { get; set; } = TankStatus.Alive;
    public int PointRegister {get; set;} = 0;
    public int DiedInTurn {get; set;} = -1;
}
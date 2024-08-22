using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;
using MessagePack;

namespace BotBattle.Engine.Models;

[MessagePackObject]
public class Obstacle
{
    [Key(0)]
    public string Id { get; set; } 
    
    [Key(1)]
    public required Position Position { get; set; }
    
    [Key(2)]
    public required ObstacleType Type { get; set; }
    
    [Key(3)]
    public int UpdateTurn { get; set; }
}
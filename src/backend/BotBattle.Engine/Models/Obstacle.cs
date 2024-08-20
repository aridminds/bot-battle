using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Models;

public class Obstacle
{
    public required Position Position { get; set; }
    public required ObstacleType Type { get; set; }
    public int UpdateTurn { get; set; }
}
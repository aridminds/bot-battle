using BotBattle.Core.Enums;

namespace BotBattle.Core;

public class Obstacle
{
    public string Id { get; set; }
    public required Position Position { get; set; }
    public required ObstacleType Type { get; set; }
    public int UpdateTurn { get; set; }
}
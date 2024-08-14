using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Models;

public class Tank
{
    public required string Name { get; init; }
    public required Position Position { get; set; }
    public int Health { get; set; } = 1000;
    public TankStatus Status { get; set; } = TankStatus.Alive;
}
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Models;

public class Position(int x, int y, Direction direction= Direction.North)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public Direction Direction { get; set; } = direction;

    public override bool Equals(object? obj)
    {
        if (obj is not Position position) return false;

        if (X != position.X) return false;
        if (Y != position.Y) return false;
        return true;
    }
}
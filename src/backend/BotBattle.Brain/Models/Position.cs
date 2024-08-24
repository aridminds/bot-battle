using MessagePack;

namespace BotBattle.Brain.Models;

[MessagePackObject]
public class Position
{
    public Position()
    {
    }

    public Position(int x, int y, Direction direction = Direction.North)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

    [Key(0)] public int X { get; set; }

    [Key(1)] public int Y { get; set; }

    [Key(2)] public Direction Direction { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Position position) return false;

        if (X != position.X) return false;
        if (Y != position.Y) return false;
        return true;
    }
}
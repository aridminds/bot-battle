namespace BotBattle.Core;

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

    public int X { get; set; }

    public int Y { get; set; }

    public Direction Direction { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Position position) return false;

        if (X != position.X) return false;
        if (Y != position.Y) return false;
        return true;
    }
}
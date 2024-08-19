using System.Globalization;
using BotBattle.Brain;
using BotBattle.Brain.Models;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Services;

public static class TankCalculator
{
    public static TankAction CalculateNextAction(string payloadHashString)
    {
        var direction = payloadHashString[..2];
        var shootingRange = payloadHashString[2..4];
        var actionDecision = payloadHashString[4..5];

        var rawShootingRange = int.Parse(shootingRange, NumberStyles.HexNumber);

        return new TankAction
        {
            Rotation = MapToDirection(int.Parse(direction, NumberStyles.HexNumber)),
            ShouldShoot = actionDecision[0] % 2 == 0,
            RawShootingRange = rawShootingRange
        };
    }

    public static int CalculateShootingRange(int rawShootingRange, int blastRadius, int gridWidth, int gridHeight,
        Position tankPosition)
    {
        var gridSize = tankPosition.Direction switch
        {
            Direction.North or Direction.South => gridHeight,
            Direction.East or Direction.West => gridWidth,
            Direction.NorthEast or Direction.SouthWest or Direction.SouthEast or Direction.NorthWest => Math.Min(
                gridWidth, gridHeight),
            _ => throw new ArgumentOutOfRangeException()
        };
        var mappedShootingRange = Math.Max(blastRadius + 1, (int)((double)rawShootingRange / 255 * gridSize));

        var maxRange = tankPosition.Direction switch
        {
            Direction.North => tankPosition.Y,
            Direction.NorthEast => Math.Min(tankPosition.Y, gridWidth - tankPosition.X),
            Direction.East => gridWidth - tankPosition.X - 1,
            Direction.SouthEast => Math.Min(gridHeight - tankPosition.Y - 1, gridWidth - tankPosition.X),
            Direction.South => gridHeight - tankPosition.Y - 1,
            Direction.SouthWest => Math.Min(gridHeight - tankPosition.Y - 1, tankPosition.X),
            Direction.West => tankPosition.X,
            Direction.NorthWest => Math.Min(tankPosition.Y, tankPosition.X),
            _ => throw new ArgumentOutOfRangeException()
        };

        return Math.Min(mappedShootingRange, maxRange);
    }

    private static Direction MapToDirection(int value)
    {
        return value switch
        {
            < 32 => Direction.North,
            < 64 => Direction.NorthEast,
            < 96 => Direction.East,
            < 128 => Direction.SouthEast,
            < 160 => Direction.South,
            < 192 => Direction.SouthWest,
            < 224 => Direction.West,
            _ => Direction.NorthWest
        };
    }
}
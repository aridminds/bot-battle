using System.Globalization;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Services;

public class TankCalculator
{
    public static TankAction CalculateNextAction(string payloadHashString, int gridWidth, int gridHeight, int blastRadius)
    {
        var direction = payloadHashString[..2];
        var shootingRange = payloadHashString[2..4];
        var actionDecision = payloadHashString[4..5];

        var rawShootingRange = int.Parse(shootingRange, NumberStyles.HexNumber);
        var gridSize = Math.Max(gridWidth, gridHeight);
        var mappedShootingRange = Math.Max(blastRadius + 1, (int)((double)rawShootingRange / 255 * gridSize));

        return new TankAction
        {
            Rotation = MapToDirection(int.Parse(direction, NumberStyles.HexNumber)),
            ShouldShoot = actionDecision[0] % 2 == 0,
            ShootingRange = mappedShootingRange
        };
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
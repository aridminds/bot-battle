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
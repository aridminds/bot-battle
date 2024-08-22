using System.Globalization;
using BotBattle.Brain.Models;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class TankCalculator
{
    public static IEnumerable<ITankAction> CalculateNextAction(string payloadHashString, Tank tank)
    {
        var direction = payloadHashString[..2];
        var shootingRange = payloadHashString[2..4];
        var actionDecision = payloadHashString[4..5];

        var rawShootingRange = int.Parse(shootingRange, NumberStyles.HexNumber);

        yield return new Rotate(MapToDirection(int.Parse(direction, NumberStyles.HexNumber)));
        yield return new Drive();
        if (actionDecision[0] % 2 == 0 && (Random.Shared.NextDouble() < 0.1d || tank.WeaponSystem.CanShoot)) yield return tank.Shoot(rawShootingRange);
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
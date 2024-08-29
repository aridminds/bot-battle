using BotBattle.Core.Enums;

namespace BotBattle.Engine.Extensions;

public static class ObstacleTypeExtensions
{
    public static bool IsBlockingMovement(this ObstacleType obstacle)
    {
        return obstacle switch
        {
            ObstacleType.OilBarrel => true,
            ObstacleType.TreeLarge => true,
            ObstacleType.Stone => true,
            _ => false
        };
    }
}
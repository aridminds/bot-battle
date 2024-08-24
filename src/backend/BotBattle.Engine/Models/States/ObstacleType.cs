namespace BotBattle.Engine.Models.States;

public enum ObstacleType
{
    Destroyed = 0,
    TreeLeaf = 1,
    TreeSmall = 2,
    TreeLarge = 3,
    Stone = 4,
    OilBarrel = 5,
    OilStain = 6
}

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
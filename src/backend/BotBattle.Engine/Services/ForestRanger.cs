using BotBattle.Core.Enums;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class ForestRanger
{
    public static void GrowUpTrees(BoardState boardState)
    {
        foreach (var obstacle in boardState.Obstacles)
        {
            if (obstacle.Type is ObstacleType.Stone or ObstacleType.TreeLarge) continue;
            if (boardState.Turns <= obstacle.UpdateTurn + 15) continue;
            if (boardState.Tanks.Any(t => t.Position.Equals(obstacle.Position))) continue;

            obstacle.Type = obstacle.Type switch
            {
                ObstacleType.Destroyed => ObstacleType.TreeLeaf,
                ObstacleType.TreeLeaf => ObstacleType.TreeSmall,
                ObstacleType.TreeSmall => ObstacleType.TreeLarge,
                _ => obstacle.Type
            };
            obstacle.UpdateTurn = boardState.Turns;
        }
    }
}
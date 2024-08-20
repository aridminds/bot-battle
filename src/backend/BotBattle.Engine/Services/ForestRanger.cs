using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Services;

public static class ForestRanger
{
    public static void GrowUpTrees(BoardState boardState)
    {
        foreach (var obstacle in boardState.Obstacles)
        {
            if(obstacle.Type == ObstacleType.Stone || obstacle.Type == ObstacleType.TreeLarge) continue;
            if (boardState.Turns <= obstacle.UpdateTurn + 15) continue;
            
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
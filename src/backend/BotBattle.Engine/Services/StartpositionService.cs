using BotBattle.Brain;
using BotBattle.Brain.Models;
using BotBattle.Engine.Helper;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class StartPositionService
{
    private const int MinDistanceBetweenPlayers = 2;

    public static Position SetStartPosition(BoardState boardState)
    {
        var random = new Random();
        Position newPosition;
        do
        {
            newPosition = new Position(random.Next(0, boardState.Width),
                random.Next(0, boardState.Height), EnumHelper.GetRandomEnumValue<Direction>());
        } while (!IsPositionValid(newPosition, boardState));

        return newPosition;
    }

    private static bool IsPositionValid(Position position, BoardState boardState)
    {
        foreach (var otherPlayer in boardState.Tanks)
            if (otherPlayer.Position.Y != 0 && otherPlayer.Position.X != 0 &&
                GetDistance(position, otherPlayer.Position) < MinDistanceBetweenPlayers)
                return false;
        
        foreach (var obstacle in boardState.Obstacles)
            if (obstacle.Position.Y != 0 && obstacle.Position.X != 0 &&
                GetDistance(position, obstacle.Position) < MinDistanceBetweenPlayers)
                return false;
        return true;
    }

    private static double GetDistance(Position p1, Position p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
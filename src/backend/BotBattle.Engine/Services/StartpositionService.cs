using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class StartPositionService
{
    private const int MinDistanceBetweenPlayers = 2;

    public static void SetStartPositionForPlayer(Tank tank, BoardState boardState)
    {
        var random = new Random();
        Position newPosition;
        do
        {
            newPosition = new Position(random.Next(0, boardState.Width),
                random.Next(0, boardState.Height));
        } while (!IsPositionValid(newPosition, boardState.Tanks));

        tank.Position = newPosition;
    }

    private static bool IsPositionValid(Position position, List<Tank> players)
    {
        foreach (var otherPlayer in players)
            if (otherPlayer.Position.Y != 0 && otherPlayer.Position.X != 0 &&
                GetDistance(position, otherPlayer.Position) < MinDistanceBetweenPlayers)
                return false;
        return true;
    }

    private static double GetDistance(Position p1, Position p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
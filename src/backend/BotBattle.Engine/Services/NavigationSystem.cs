using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Extensions;
using BotBattle.Engine.Models;


namespace BotBattle.Engine.Services;

public static class NavigationSystem
{
    private const int AwayFromBorderOffset = 0;


    public static Position CalculateNewPosition(BoardState boardState, Position currentPosition, Direction direction)
    {
        var newPosition = CalculateNewPosition(currentPosition, direction);
        var possibleNewPosition = ClampPositionToGrid(newPosition, boardState);
        possibleNewPosition.Direction = direction;
        return possibleNewPosition;
    }

    public static bool IsPositionOccupied(Position position, BoardState boardState)
    {
        return boardState.Tanks.Any(player => player.Position.Equals(position))
               || boardState.Obstacles.Any(obstacle =>
                   obstacle.Position.Equals(position) && obstacle.Type.IsBlockingMovement());
    }

    public static bool IsDroveIntoAnOilStain(Position position, BoardState boardState)
    {
        return boardState.Obstacles.Any(obstacle =>
            obstacle.Type == ObstacleType.OilStain && obstacle.Position.Equals(position));
    }

    private static Position CalculateNewPosition(Position currentPosition, Direction direction)
    {
        return direction switch
        {
            Direction.North => new Position(currentPosition.X, currentPosition.Y - 1, direction),
            Direction.NorthEast => new Position(currentPosition.X + 1, currentPosition.Y - 1, direction),
            Direction.East => new Position(currentPosition.X + 1, currentPosition.Y, direction),
            Direction.SouthEast => new Position(currentPosition.X + 1, currentPosition.Y + 1, direction),
            Direction.South => new Position(currentPosition.X, currentPosition.Y + 1, direction),
            Direction.SouthWest => new Position(currentPosition.X - 1, currentPosition.Y + 1, direction),
            Direction.West => new Position(currentPosition.X - 1, currentPosition.Y, direction),
            Direction.NorthWest => new Position(currentPosition.X - 1, currentPosition.Y - 1, direction),
            _ => currentPosition
        };
    }

    private static Position ClampPositionToGrid(Position position, BoardState boardState)
    {
        position.X = Math.Clamp(position.X, AwayFromBorderOffset, boardState.Width - 1 - AwayFromBorderOffset);
        position.Y = Math.Clamp(position.Y, AwayFromBorderOffset, boardState.Height - 1 - AwayFromBorderOffset);
        return position;
    }
}
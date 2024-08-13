using BotBattle.Engine.Models;
using BotBattle.Engine.Services;

namespace BotBattle.Engine;

public class GameMaster
{
    private const int AwayFromBorderOffset = 0;
    private const int BlastRadius = 3;
    private const int FullBulletHit = 30;

    public BoardState NextRound(string payloadHashString, BoardState boardState, Tank tank)
    {
        boardState.Bullets.Clear();
        var tankAction = TankCalculator.CalculateNextAction(payloadHashString, boardState.Width, boardState.Height, BlastRadius);
        var currentTank = boardState.Tanks.First(t => t.Name == tank.Name);
        if (currentTank.Status == TankStatus.Dead) return boardState;

        currentTank.Position = MoveTank(tankAction.Rotation, currentTank.Position, boardState);

        if (tankAction.ShouldShoot)
            ShootBullet(tankAction.Rotation, tankAction.ShootingRange, currentTank, boardState);
        CalculateIsSomeoneHit(boardState.Tanks, boardState);

        CheckForWinner(boardState);
        
        return boardState;
    }
    
    public void CheckForWinner(BoardState boardState)
    {
        var alivePlayers = boardState.Tanks.Where(player => player.Status == TankStatus.Alive).ToList();

        if (alivePlayers.Count == 1)
        {
            var winner = alivePlayers.First();
            winner.Status = TankStatus.Winner;
            boardState.Status = GameStatus.GameOver;
        }
    }

    private void CalculateIsSomeoneHit(List<Tank> players, BoardState boardState)
    {
        foreach (var player in players)
        {
            if (player.Status == TankStatus.Dead) continue;
            foreach (var bullet in boardState.Bullets)
            {
                var distance = CalculateDistance(bullet.Target, player.Position);
                if (!(distance <= BlastRadius)) continue;
                var healthReduction = CalculateHealthReduction(distance);
                player.Health -= healthReduction;
            }

            if (player.Health > 0) continue;

            player.Status = TankStatus.Dead;
            player.Health = 0;
        }
    }

    private double CalculateDistance(Position pos1, Position pos2)
    {
        var xDistance = pos1.X - pos2.X;
        var yDistance = pos1.Y - pos2.Y;
        return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
    }

    private int CalculateHealthReduction(double distance)
    {
        return (int)(FullBulletHit * (1 - distance / BlastRadius));
    }

    private void ShootBullet(Direction direction, int shootingRange, Tank currentTank, BoardState boardState)
    {
        var bulletPosition = currentTank.Position;

        for (var i = 0; i < shootingRange; i++)
        {
            bulletPosition = CalculateNewPosition(bulletPosition, direction);
            bulletPosition = ClampPositionToGrid(bulletPosition, boardState);
        }

        if (!bulletPosition.Equals(currentTank.Position))
            boardState.Bullets.Add(new Bullet { Source = currentTank.Position, Target = bulletPosition, Shooter = currentTank});
    }


    private Position MoveTank(Direction direction, Position currentTankPosition, BoardState boardState)
    {
        var newPosition = CalculateNewPosition(currentTankPosition, direction);
        var possibleNewPosition = ClampPositionToGrid(newPosition, boardState);
        possibleNewPosition.Direction = direction;

        return !IsPositionOccupied(possibleNewPosition, boardState) ? possibleNewPosition : currentTankPosition;
    }

    private bool IsPositionOccupied(Position position, BoardState boardState)
    {
        return boardState.Tanks.Any(player => player.Position.Equals(position));
    }

    private Position CalculateNewPosition(Position currentPosition, Direction direction)
    {
        return direction switch
        {
            Direction.North => new Position(currentPosition.X, currentPosition.Y - 1),
            Direction.NorthEast => new Position(currentPosition.X + 1, currentPosition.Y - 1),
            Direction.East => new Position(currentPosition.X + 1, currentPosition.Y),
            Direction.SouthEast => new Position(currentPosition.X + 1, currentPosition.Y + 1),
            Direction.South => new Position(currentPosition.X, currentPosition.Y + 1),
            Direction.SouthWest => new Position(currentPosition.X - 1, currentPosition.Y + 1),
            Direction.West => new Position(currentPosition.X - 1, currentPosition.Y),
            Direction.NorthWest => new Position(currentPosition.X - 1, currentPosition.Y - 1),
            _ => currentPosition
        };
    }

    private Position ClampPositionToGrid(Position position, BoardState boardState)
    {
        position.X = Math.Clamp(position.X, AwayFromBorderOffset, boardState.Width - 1 - AwayFromBorderOffset);
        position.Y = Math.Clamp(position.Y, AwayFromBorderOffset, boardState.Height - 1 - AwayFromBorderOffset);
        return position;
    }
}
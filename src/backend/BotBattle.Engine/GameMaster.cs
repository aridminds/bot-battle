using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;

namespace BotBattle.Engine;

public class GameMaster
{
    private const int AwayFromBorderOffset = 0;
    private const int BlastRadius = 3;
    private const int FullBulletHit = 30;

    public BoardState NextRound(string payloadHashString, BoardState boardState, Tank tank)
    {
        boardState.Turns++;
        for (var index = 0; index < boardState.Bullets.Count; index++)
        {
            var bullet = boardState.Bullets[index];

            switch (bullet.ShootingRange)
            {
                case 0:
                    bullet.ShootingRange--;
                    bullet.Status = BulletStatus.Hit;
                    break;
                case < 0:
                    boardState.Bullets.RemoveAt(index);
                    break;
                default:
                    bullet.CurrentPosition =
                        CalculateNewPosition(bullet.CurrentPosition, bullet.CurrentPosition.Direction);
                    bullet.ShootingRange--;
                    IsSomeoneDirectlyHit(boardState, bullet);
                    break;
            }
        }

        var tankAction = TankCalculator.CalculateNextAction(payloadHashString);
        var currentTank = boardState.Tanks.First(t => t.Name == tank.Name);
        if (currentTank.Status == TankStatus.Dead) return boardState;

        currentTank.Position = MoveTank(tankAction.Rotation, currentTank.Position, boardState);

        if (tankAction.ShouldShoot)
        {
            var shootingRange = TankCalculator.CalculateShootingRange(tankAction.RawShootingRange, BlastRadius,
                boardState.Width, boardState.Height, currentTank.Position);
            ShootBullet(shootingRange, currentTank, boardState);
        }


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
            boardState.EventLogs.Add(EventLogExtensions.CreateHasWonEventLog(boardState.Turns, winner));
        }
    }

    private static void IsSomeoneDirectlyHit(BoardState boardState, Bullet bullet)
    {
        if (bullet.ShootingRange <= 0) return;

        foreach (var player in boardState.Tanks)
        {
            if (player.Status == TankStatus.Dead) continue;
            if (player == bullet.Shooter) continue;
            if (!bullet.CurrentPosition.Equals(player.Position)) continue;

            bullet.ShootingRange = -1;
            bullet.Status = BulletStatus.Hit;
            CheckPlayerHealthAndCrateEventLog(boardState, player, bullet, FullBulletHit, true);
        }
    }

    private void CalculateIsSomeoneHit(List<Tank> players, BoardState boardState)
    {
        foreach (var player in players)
        {
            if (player.Status == TankStatus.Dead) continue;
            foreach (var bullet in boardState.Bullets)
            {
                if (bullet.Status != BulletStatus.Hit) break;
                var distance = CalculateDistance(bullet.CurrentPosition, player.Position);
                if (!(distance <= BlastRadius)) continue;
                var healthReduction = CalculateHealthReduction(distance);
                CheckPlayerHealthAndCrateEventLog(boardState, player, bullet, healthReduction);
            }
        }
    }

    private static void CheckPlayerHealthAndCrateEventLog(BoardState boardState, Tank player, Bullet bullet,
        int healthReduction, bool directHit = false)
    {
        player.Health -= FullBulletHit;
        if (player.Health <= 0)
        {
            player.Status = TankStatus.Dead;
            player.Health = 0;
            boardState.EventLogs.Add(
                EventLogExtensions.CreateKillEventLog(boardState.Turns, bullet.Shooter, player, directHit));
        }
        else
        {
            boardState.EventLogs.Add(EventLogExtensions.CreateHitEventLog(boardState.Turns, bullet.Shooter, player,
                healthReduction, directHit));
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

    private void ShootBullet(int shootingRange, Tank currentTank, BoardState boardState)
    {
        var bullet = new Bullet
        {
            Id = $"{currentTank.Name}-{Guid.NewGuid()}", 
            Shooter = currentTank,
            CurrentPosition = currentTank.Position, 
            ShootingRange = shootingRange - 1
        };
        
        bullet.CurrentPosition = CalculateNewPosition(bullet.CurrentPosition, bullet.CurrentPosition.Direction);
        boardState.Bullets.Add(bullet);
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

    private Position ClampPositionToGrid(Position position, BoardState boardState)
    {
        position.X = Math.Clamp(position.X, AwayFromBorderOffset, boardState.Width - 1 - AwayFromBorderOffset);
        position.Y = Math.Clamp(position.Y, AwayFromBorderOffset, boardState.Height - 1 - AwayFromBorderOffset);
        return position;
    }
}
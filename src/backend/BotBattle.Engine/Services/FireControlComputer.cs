using BotBattle.Brain.Models;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Services;

public static class FireControlComputer
{
    private const int BlastRadius = 3;
    public const int FullBulletHit = 30;

    public static void DealDamage(Tank source, Tank target, int amount, HitType hitType, BoardState boardState)
    {
        target.Health -= amount;
        if (target.Health <= 0)
        {
            target.Status = TankStatus.Dead;
            target.Health = 0;
            target.DiedInTurn = boardState.Turns;
            PointJudge.CalculatePoints(source, amount, source.Name == target.Name, true, boardState);
            boardState.EventLogs.Add(
                EventLogExtensions.CreateKillEventLog(boardState.Turns, source, target, hitType));
        }
        else
        {
            PointJudge.CalculatePoints(source, amount, source.Name == target.Name, false, boardState);
            boardState.EventLogs.Add(EventLogExtensions.CreateHitEventLog(boardState.Turns, source, target,
                amount, hitType));
        }
    }

    public static void ShootBullet(int shootingPower, Tank currentTank, BoardState boardState)
    {
        var shootingRange =
            CalculateShootingRange(shootingPower, boardState.Width, boardState.Height, currentTank.Position);

        var bullet = new Bullet
        {
            Id = $"{currentTank.Name}-{Guid.NewGuid()}",
            Shooter = currentTank,
            CurrentPosition = currentTank.Position,
            ShootingRange = shootingRange - 1
        };

        bullet.CurrentPosition =
            NavigationSystem.CalculateNewPosition(boardState, bullet.CurrentPosition, bullet.CurrentPosition.Direction);
        boardState.Bullets.Add(bullet);
    }

    public static void CheckBullets(BoardState boardState)
    {
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
                        NavigationSystem.CalculateNewPosition(boardState, bullet.CurrentPosition,
                            bullet.CurrentPosition.Direction);
                    bullet.ShootingRange--;
                    break;
            }
        }
    }

    public static void CalculateIsSomeoneHit(BoardState boardState)
    {
        foreach (var bullet in boardState.Bullets)
        {
            foreach (var tank in boardState.Tanks)
            {
                if (tank.Status == TankStatus.Dead) continue;
                if (bullet.CurrentPosition.Equals(tank.Position))
                {
                    if (tank == bullet.Shooter) continue;
                    bullet.ShootingRange = -1;
                    bullet.Status = BulletStatus.Hit;
                    DealDamage(bullet.Shooter, tank, FullBulletHit, HitType.Bullet, boardState);
                }
                else
                {
                    if (bullet.Status != BulletStatus.Hit && bullet.Status != BulletStatus.SuperHit) continue;
                    bullet.ShootingRange = -1;
                    var distance = CalculateDistance(bullet.CurrentPosition, tank.Position);
                  
                    var blastRadius =  bullet.Status == BulletStatus.SuperHit ? 5 : BlastRadius;
                    if (!(distance < blastRadius)) continue;
                    var healthReduction = CalculateHealthReduction(distance, blastRadius);
                    DealDamage(bullet.Shooter, tank, healthReduction, HitType.BulletBlast, boardState);
                }
            }

            foreach (var obstacle in boardState.Obstacles)
            {
                if (!bullet.CurrentPosition.Equals(obstacle.Position)) continue;
                if (obstacle.Type is ObstacleType.Destroyed or ObstacleType.TreeSmall or ObstacleType.TreeLeaf or ObstacleType.OilStain) continue;
                
                bullet.ShootingRange = -1;
                bullet.Status = BulletStatus.Hit;
                if (obstacle.Type == ObstacleType.Stone) continue;
                if (obstacle.Type == ObstacleType.OilBarrel) bullet.Status = BulletStatus.SuperHit;

                obstacle.Type = obstacle.Type switch
                {
                    ObstacleType.TreeLarge => ObstacleType.Destroyed,
                    ObstacleType.OilBarrel => ObstacleType.OilStain,
                    _ => ObstacleType.Destroyed
                };

                obstacle.UpdateTurn = boardState.Turns;
            }
        }
    }

    public static int CalculateDistance(Position pos1, Position pos2)
    {
        var xDistance = Math.Abs(pos1.X - pos2.X);
        var yDistance = Math.Abs(pos1.Y - pos2.Y);
        return Math.Max(xDistance, yDistance);
    }

    public static int CalculateHealthReduction(double distance, int blastRadius = BlastRadius)
    {
        return (int)(FullBulletHit * (1 - distance / blastRadius));
    }
  
    public static int CalculateShootingRange(int shootingPower, int gridWidth, int gridHeight,
        Position tankPosition)
    {
        var gridSize = tankPosition.Direction switch
        {
            Direction.North or Direction.South => gridHeight,
            Direction.East or Direction.West => gridWidth,
            Direction.NorthEast or Direction.SouthWest or Direction.SouthEast or Direction.NorthWest => Math.Min(
                gridWidth, gridHeight),
            _ => throw new ArgumentOutOfRangeException()
        };
        var mappedShootingRange = Math.Max(BlastRadius + 1, (int)((double)shootingPower / 255 * gridSize));

        var maxRange = tankPosition.Direction switch
        {
            Direction.North => tankPosition.Y,
            Direction.NorthEast => Math.Min(tankPosition.Y - 1, gridWidth - tankPosition.X),
            Direction.East => gridWidth - tankPosition.X - 1,
            Direction.SouthEast => Math.Min(gridHeight - tankPosition.Y - 1, gridWidth - tankPosition.X),
            Direction.South => gridHeight - tankPosition.Y - 1,
            Direction.SouthWest => Math.Min(gridHeight - tankPosition.Y - 1, tankPosition.X),
            Direction.West => tankPosition.X,
            Direction.NorthWest => Math.Min(tankPosition.Y, tankPosition.X),
            _ => throw new ArgumentOutOfRangeException()
        };

        var min = Math.Min(mappedShootingRange, maxRange);
        return min >= 0 ? min : 0;
    }
}
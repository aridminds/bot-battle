using BotBattle.AgentLib;
using BotBattle.AgentRunner;
using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services;
using Drive = BotBattle.AgentLib.Drive;
using Position = BotBattle.Core.Position;
using Rotate = BotBattle.AgentLib.Rotate;
using Shoot = BotBattle.AgentLib.Shoot;
using Tank = BotBattle.Core.Tank;

namespace BotBattle.Engine;

public class GameMaster
{
    public static void NextRound(BoardState boardState, Tank tank, WasmRunner wasmRunner)
    {
        boardState.Turns++;

        FireControlComputer.CheckBullets(boardState);

        if (tank.WeaponSystem.ActiveFireCooldown > 0)
            tank.WeaponSystem = tank.WeaponSystem with
            {
                ActiveFireCooldown = tank.WeaponSystem.ActiveFireCooldown - 1
            };

        var tankAction = wasmRunner.Execute(tank.Name, new Arena
        {
            Width = boardState.Width,
            Height = boardState.Height,
            Turn = boardState.Turns,
            Tanks = boardState.Tanks.Select(t => new BotBattle.AgentLib.Tank
            {
                Name = t.Name,
                Health = t.Health,
                Position = new BotBattle.AgentLib.Position
                {
                    X = t.Position.X,
                    Y = t.Position.Y,
                },
                Direction = (AgentLib.Enums.Direction)t.Position.Direction,
                WeaponSystem = new AgentLib.WeaponSystem
                {
                    FireCooldown = t.WeaponSystem.FireCooldown,
                    ActiveFireCooldown = t.WeaponSystem.ActiveFireCooldown
                }
            }).ToList(),
            Bullets = boardState.Bullets.Select(bullet => new BotBattle.AgentLib.Bullet
            {
                Position = new BotBattle.AgentLib.Position
                {
                    X = bullet.CurrentPosition.X,
                    Y = bullet.CurrentPosition.Y
                },
                Direction = (AgentLib.Enums.Direction)bullet.CurrentPosition.Direction,
                Owner = new AgentLib.Tank
                {
                    Name = bullet.Shooter.Name,
                    Health = bullet.Shooter.Health,
                    Position = new BotBattle.AgentLib.Position
                    {
                        X = bullet.Shooter.Position.X,
                        Y = bullet.Shooter.Position.Y
                    },
                    Direction = (AgentLib.Enums.Direction)bullet.Shooter.Position.Direction,
                    WeaponSystem = new AgentLib.WeaponSystem
                    {
                        FireCooldown = bullet.Shooter.WeaponSystem.FireCooldown,
                        ActiveFireCooldown = bullet.Shooter.WeaponSystem.ActiveFireCooldown
                    }
                },
            }).ToList(),
            Obstacles = boardState.Obstacles.Select(obstacle => new BotBattle.AgentLib.Obstacle
            {
                Position = new BotBattle.AgentLib.Position
                {
                    X = obstacle.Position.X,
                    Y = obstacle.Position.Y
                }
            }).ToList()
        });

        switch (tankAction.Action)
        {
            case Rotate rotate:
                tank.Position.Direction = (Direction)rotate.Direction;
                break;
            case Drive drive:
                if (tank.Status == TankStatus.IsStucked)
                {
                    boardState.EventLogs.Add(EventLogExtensions.CreateIsStuckedEventLog(boardState.Turns, tank));
                    tank.Status = tank.Health > 0 ? TankStatus.Alive : TankStatus.Dead;
                }
                else
                {
                    tank.Position = MoveTank(tank.Position.Direction, tank.Position, boardState);
                    if (NavigationSystem.IsDroveIntoAnOilStain(tank.Position, boardState))
                    {
                        tank.Status = TankStatus.IsStucked;
                    }
                }

                break;
            case Shoot shoot:
                if (!tank.WeaponSystem.CanShoot)
                {
                    if (Random.Shared.NextDouble() < .2d)
                    {
                        FireControlComputer.DealDamage(tank, tank, FireControlComputer.FullBulletHit,
                            HitType.JamExplosion, boardState);
                        break;
                    }

                    boardState.EventLogs.Add(new EventLog
                        { Message = $"{tank.Name} tried to shoot while reloading.", Turn = boardState.Turns });
                    break;
                }

                FireControlComputer.ShootBullet(shoot.Power, tank, boardState);
                tank.WeaponSystem = tank.WeaponSystem with { ActiveFireCooldown = tank.WeaponSystem.FireCooldown };
                break;
        }

        FireControlComputer.CalculateIsSomeoneHit(boardState);
        CheckForWinner(boardState);
        ForestRanger.GrowUpTrees(boardState);
    }

    private static void CheckForWinner(BoardState boardState)
    {
        var alivePlayers = boardState.Tanks.Where(player => player.Health > 0).ToList();
        if (alivePlayers.Count != 1) return;

        var winner = alivePlayers.First();
        winner.Status = TankStatus.Winner;
        boardState.Status = GameStatus.GameOver;
        boardState.EventLogs.Add(EventLogExtensions.CreateHasWonEventLog(boardState.Turns, winner));
        boardState.Bullets.Clear();
    }

    private static Position MoveTank(Direction direction, Position currentTankPosition, BoardState boardState)
    {
        var possibleNewPosition = NavigationSystem.CalculateNewPosition(boardState, currentTankPosition, direction);
        return !NavigationSystem.IsPositionOccupied(possibleNewPosition, boardState)
            ? possibleNewPosition
            : currentTankPosition;
    }
}
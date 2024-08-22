using BotBattle.Brain.Models;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;

namespace BotBattle.Engine;

public class GameMaster
{
    public static void NextRound(string payloadHashString, BoardState boardState, Tank tank)
    {
        boardState.Turns++;

        FireControlComputer.CheckBullets(boardState);

        if (tank.WeaponSystem.ActiveFireCooldown > 0)
            tank.WeaponSystem = tank.WeaponSystem with
            {
                ActiveFireCooldown = tank.WeaponSystem.ActiveFireCooldown - 1
            };

        if (tank.Status == TankStatus.IsStucked)
        {
            boardState.EventLogs.Add(EventLogExtensions.CreateIsStuckedEventLog(boardState.Turns, tank));
            tank.Status = tank.Health > 0 ? TankStatus.Alive : TankStatus.Dead;
        }

        foreach (var action in TankCalculator.CalculateNextAction(payloadHashString, tank))
        {
            switch (action)
            {
                case Rotate rotate:
                    tank.Position.Direction = rotate.Direction;
                    break;
                case Drive:
                    if (tank.Status == TankStatus.IsStucked)
                        break;

                    tank.Position = MoveTank(tank.Position.Direction, tank.Position, boardState);
                    if (NavigationSystem.IsDroveIntoAnOilStain(tank.Position, boardState))
                    {
                        tank.Status = TankStatus.IsStucked;
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
        }

        FireControlComputer.CalculateIsSomeoneHit(boardState);
        CheckForWinner(boardState);
        ForestRanger.GrowUpTrees(boardState);
    }

    private static void CheckForWinner(BoardState boardState)
    {
        var alivePlayers = boardState.Tanks.Where(player => player.Status == TankStatus.Alive).ToList();
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
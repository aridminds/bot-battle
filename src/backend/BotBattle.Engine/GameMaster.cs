using BotBattle.Brain.Models;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;

namespace BotBattle.Engine;

public class GameMaster
{
    public BoardState NextRound(string payloadHashString, BoardState boardState, Tank tank)
    {
        boardState.Turns++;

        FireControlComputer.CheckBullets(boardState);
        var tankAction = TankCalculator.CalculateNextAction(payloadHashString);
        var currentTank = boardState.Tanks.First(t => t.Name == tank.Name);
        if (currentTank.Status == TankStatus.Dead) return boardState;

        currentTank.Position = MoveTank(tankAction.Rotation, currentTank.Position, boardState);

        if (tankAction.ShouldShoot)
            FireControlComputer.ShootBullet(tankAction.RawShootingRange, currentTank, boardState);

        FireControlComputer.CalculateIsSomeoneHit(boardState);
        CheckForWinner(boardState);
        ForestRanger.GrowUpTrees(boardState);

        return boardState;
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
using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class PointJudge
{
    private const int FullHitPoints = 9;
    
    public static void CalculatePoints(Tank shootingTank, int healthReduction, bool isSelfHit, bool isKill, BoardState boardState)
    {
        var points = (int)((double)healthReduction / FireControlComputer.FullBulletHit * FullHitPoints);
        if (isKill)
        {
            var survivedTanksCount = boardState.Tanks.Count(t => t.Status == TankStatus.Dead && t.DiedInTurn < boardState.Turns);
            points *= 2;
            points += survivedTanksCount * 10;
            
        }
        if(isSelfHit) points = -points;
        shootingTank.PointRegister += points;
        
        if(shootingTank.PointRegister < 0) shootingTank.PointRegister = 0;
    }
}
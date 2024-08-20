using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class PointJudge
{
    private const int FullHitPoints = 9;
    
    public static void CalculatePoints(Tank shootingTank, int healthReduction, bool isSelfHit, bool isKill)
    {
        var points = (int)((double)healthReduction / FireControlComputer.FullBulletHit * FullHitPoints);
        if(isKill) points *= 2;
        if(isSelfHit) points = -points;
        shootingTank.PointRegister += points;
    }
}
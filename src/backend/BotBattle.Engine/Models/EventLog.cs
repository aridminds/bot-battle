using BotBattle.Engine.Services;

namespace BotBattle.Engine.Models;

public record EventLog(string Message, int Turn);

public static class EventLogExtensions
{
    public static EventLog CreateHitEventLog(int turn, Tank shooter, Tank target, int healthReduction, HitType hitType)
    {
        var directHitMessage = hitType == HitType.Bullet ? "directly " : "";
        return new EventLog($"{shooter.Name} {directHitMessage}hit {target.Name} (-{healthReduction}) ({hitType.TranslateHitType()})", turn);
    }

    public static EventLog CreateKillEventLog(int turn, Tank shooter, Tank target, HitType hitType)
    {
        var directHitMessage = hitType == HitType.Bullet ? "directly " : "";
        return new EventLog($"{shooter.Name} {directHitMessage}killed {target.Name} ({hitType.TranslateHitType()})", turn);
    }

    public static EventLog CreateHasWonEventLog(int turn, Tank winner)
    {
        return new EventLog($"{winner.Name} has won the game!", turn);
    }

    public static string TranslateHitType(this HitType hitType)
    {
        return hitType switch
        {
            HitType.Bullet => "Bullet",
            HitType.BulletBlast => "Blast",
            HitType.JamExplosion => "Jammed ammunition",
            _ => "Unknown",
        };

    }
}
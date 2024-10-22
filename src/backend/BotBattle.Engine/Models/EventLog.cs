using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Services;
using MessagePack;


namespace BotBattle.Engine.Models;

[MessagePackObject]
public class EventLog
{
    [Key(0)] public required string Message { get; init; }

    [Key(1)] public int Turn { get; init; }
}

public static class EventLogExtensions
{
    public static EventLog CreateHitEventLog(int turn, Tank shooter, Tank target, int healthReduction, HitType hitType)
    {
        var directHitMessage = hitType == HitType.Bullet ? "directly " : "";
        return new EventLog
        {
            Message =
                $"{shooter.Name} {directHitMessage}hit {target.Name} for {healthReduction} damage ({hitType.TranslateHitType()})",
            Turn = turn
        };
    }

    public static EventLog CreateKillEventLog(int turn, Tank shooter, Tank target, HitType hitType)
    {
        var directHitMessage = hitType == HitType.Bullet ? "directly " : "";
        return new EventLog
        {
            Message = $"{shooter.Name} {directHitMessage}killed {target.Name} ({hitType.TranslateHitType()})",
            Turn = turn
        };
    }

    public static EventLog CreateHasUsedCollectibleEventLog(int turn, Tank tank, CollectibleItemType collectibleItemType)
    {
        return new EventLog
        {
            Message = $"{tank.Name} has used {collectibleItemType}",
            Turn = turn
        };
    }
    
    public static EventLog CreateHasHasCollectedEventLog(int turn, Tank tank, CollectibleItemType collectibleItemType)
    {
        return new EventLog
        {
            Message = $"{tank.Name} has collected {collectibleItemType}",
            Turn = turn
        };
    }
    
    public static EventLog CreatePlainHasStartedEventLog(int turn)
    {
        return new EventLog
        {
            Message = "The plane has taken off",
            Turn = turn
        };
    }
    
    public static EventLog CreateHasWonEventLog(int turn, Tank winner)
    {
        return new EventLog
        {
            Message = $"{winner.Name} has won the game",
            Turn = turn
        };
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

    public static EventLog CreateIsStuckedEventLog(int turn, Tank stuckedTank)
    {
        return new EventLog
        {
            Message = $"{stuckedTank.Name} is stucked",
            Turn = turn
        };
    }
}
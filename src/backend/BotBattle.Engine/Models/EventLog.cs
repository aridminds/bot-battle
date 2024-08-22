using MessagePack;

namespace BotBattle.Engine.Models;

[MessagePackObject]
public class EventLog
{
    [Key(0)] public string Message { get; init; }

    [Key(1)] public int Turn { get; init; }
}

public static class EventLogExtensions
{
    public static EventLog CreateHitEventLog(int turn, Tank shooter, Tank target, int healthReduction,
        bool directHit = false)
    {
        var directHitMessage = directHit ? "directly " : "";
        return new EventLog
        {
            Message = $"{shooter.Name} {directHitMessage}hit {target.Name} for {healthReduction} damage",
            Turn = turn
        };
    }

    public static EventLog CreateKillEventLog(int turn, Tank shooter, Tank target, bool directHit = false)
    {
        var directHitMessage = directHit ? "directly " : "";
        return new EventLog
        {
            Message = $"{shooter.Name} {directHitMessage}killed {target.Name}",
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
}
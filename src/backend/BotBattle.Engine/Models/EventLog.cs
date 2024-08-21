namespace BotBattle.Engine.Models;

public record EventLog(string Message, int Turn);

public static class EventLogExtensions
{
    public static EventLog CreateHitEventLog(int turn, Tank shooter, Tank target, int healthReduction, bool directHit = false)
    {
        var directHitMessage = directHit ? "directly " : "";
        return new EventLog ($"{shooter.Name} {directHitMessage}hit {target.Name} (-{healthReduction})" , turn);
    }

    public static EventLog CreateKillEventLog(int turn, Tank shooter, Tank target, bool directHit = false)
    { 
        var directHitMessage = directHit ? "directly " : "";
        return new EventLog ($"{shooter.Name} {directHitMessage}killed {target.Name}" , turn);
    }
    
    public static EventLog CreateHasWonEventLog(int turn, Tank winner)
    {
        return new EventLog ($"{winner.Name} has won the game!" , turn);
    }
    
    public static EventLog CreateIsStuckedEventLog(int turn, Tank stuckedTank)
    {
        return new EventLog ($"{stuckedTank.Name} is stucked" , turn);
    }
    
}
using BotBattle.Brain.Models;

namespace BotBattle.Brain;

public interface ITankCalculator
{
    public TankAction CalculateNextAction();
}
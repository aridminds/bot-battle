using BotBattle.Brain.Models;
using BotBattle.Engine.Models;

namespace BotBattle.Brain;

public static class TankExtensions
{
    public static Shoot Shoot(this Tank tank, int power)
    {
        return new Shoot(power, tank.WeaponSystem.Id);
    }
}

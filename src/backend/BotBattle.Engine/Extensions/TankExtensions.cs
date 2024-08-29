using BotBattle.Core;

namespace BotBattle.Engine.Extensions;

public static class TankExtensions
{
    public static Shoot Shoot(this Tank tank, int power)
    {
        return new Shoot(power, tank.WeaponSystem.Id);
    }
}

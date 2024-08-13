namespace BotBattle.Engine.Models;

public struct TankAction
{
    public Direction Rotation { get; init; }
    public bool ShouldShoot { get; init; }
    public int ShootingRange { get; init; }
}
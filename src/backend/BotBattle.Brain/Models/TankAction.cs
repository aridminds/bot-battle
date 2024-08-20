namespace BotBattle.Brain.Models;

public struct TankAction
{
    public Direction Rotation { get; init; }
    public bool ShouldShoot { get; init; }
    public int RawShootingRange { get; init; }
}
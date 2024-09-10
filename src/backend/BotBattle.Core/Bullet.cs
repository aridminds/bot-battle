using BotBattle.Core.Enums;

namespace BotBattle.Core;

public class Bullet
{
    public required string Id { get; init; }
    public required Tank Shooter { get; init; }
    public required Position CurrentPosition { get; set; }
    public required int ShootingRange { get; set; }
    public BulletStatus Status { get; set; } = BulletStatus.ShotStart;
    public BulletType Type { get; init; }
}
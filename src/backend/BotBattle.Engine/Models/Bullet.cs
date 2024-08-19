using BotBattle.Brain;
using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Models;

public class Bullet
{
    public required string Id { get; init; }
    public required Tank Shooter { get; init; }
    public required Position CurrentPosition { get; set; }
    public required int ShootingRange { get; set; }
    public BulletStatus Status { get; set; } = BulletStatus.ShotStart;
}
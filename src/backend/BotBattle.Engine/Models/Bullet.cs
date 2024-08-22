using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;
using MessagePack;

namespace BotBattle.Engine.Models;

[MessagePackObject]
public class Bullet
{
    [Key(0)]
    public required string Id { get; init; }
    
    [Key(1)]
    public required Tank Shooter { get; init; }
    
    [Key(2)]
    public required Position CurrentPosition { get; set; }
    
    [Key(3)]
    public required int ShootingRange { get; set; }
    
    [Key(4)]
    public BulletStatus Status { get; set; } = BulletStatus.ShotStart;
}
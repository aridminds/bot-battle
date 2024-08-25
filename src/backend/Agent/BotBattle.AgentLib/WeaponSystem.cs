using BotBattle.AgentLib.Enums;

namespace BotBattle.AgentLib;

public class WeaponSystem
{
    public Guid Id { get; init; }
    public BulletType Bullet { get; init; }
    public float FireCooldown { get; init; }
    public float ActiveFireCooldown { get; init; }
    public bool CanShoot => ActiveFireCooldown <= 0;
}
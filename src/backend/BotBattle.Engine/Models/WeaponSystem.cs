namespace BotBattle.Engine.Models;

public readonly struct WeaponSystem
{
    public Guid Id { get; init; }
    public BulletType Bullet { get; init; }
    public float FireCooldown { get; init; }


    public float ActiveFireCooldown { get; init; }
    public readonly bool CanShoot => ActiveFireCooldown <= 0;
}

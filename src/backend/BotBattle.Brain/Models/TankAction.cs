namespace BotBattle.Brain.Models;

public interface ITankAction
{
    Guid Id { get; }
}

public readonly record struct Shoot(int Power, Guid Weapon) : ITankAction
{
    public Guid Id { get; } = Guid.NewGuid();
}

public readonly record struct Drive() : ITankAction
{
    public Guid Id { get; } = Guid.NewGuid();
}

public readonly record struct Rotate(Direction Direction) : ITankAction
{
    public Guid Id { get; } = Guid.NewGuid();
}
using BotBattle.AgentLib.Enums;

namespace BotBattle.AgentLib;

public class Arena
{
    public int Width { get; init; }
    public int Height { get; init; }
    public int Turn { get; set; } = 0;
    public List<Tank> Tanks { get; init; } = [];
    public List<Bullet> Bullets { get; init; } = [];
    public List<Obstacle> Obstacles { get; init; } = [];
    public List<CollectibleItem> CollectibleItems { get; init; } = [];
}
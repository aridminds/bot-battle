using BotBattle.Core;
using BotBattle.Core.Enums;
using MessagePack;

namespace BotBattle.Engine.Models;

[MessagePackObject]
public class BoardState
{
    public BoardState()
    {
    }

    public BoardState(int width, int height)
    {
        Width = width;
        Height = height;
    }

    [Key(0)]
    public int Width { get; init; }
    
    [Key(1)]
    public int Height { get; init; }
    
    [Key(2)]
    public List<Tank> Tanks { get; init; } = [];
    
    [Key(3)]
    public List<Bullet> Bullets { get; init; } = [];
    
    [Key(4)]
    public GameStatus Status { get; set; } = GameStatus.InProgress;
    
    [Key(5)]
    public List<EventLog> EventLogs { get; init; } = [];
    
    [Key(6)]
    public int Turns { get; set; } = 0;
    
    [Key(7)]
    public List<Obstacle> Obstacles { get; init; } = [];
    
    [Key(8)]
    public List<CollectibleItem> CollectibleItems { get; init; } = [];

    [Key(9)] 
    public Airplane Airplane { get; init; } = new();
}
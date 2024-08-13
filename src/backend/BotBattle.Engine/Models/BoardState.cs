namespace BotBattle.Engine.Models;

public class BoardState
{
    public BoardState(int width, int height)
    {
        Width = width;
        Height = height;
        // Map = MapGeneratorService.Generate(width, height);
    }

    public int Width { get; init; }
    public int Height { get; init; }
    public List<Tank> Tanks { get; init; } = [];
    public List<Bullet> Bullets { get; init; } = [];
    public GameStatus Status { get; set; } = GameStatus.InProgress;
    // public Map Map { get; set; }
}
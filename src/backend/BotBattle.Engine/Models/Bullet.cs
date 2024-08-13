namespace BotBattle.Engine.Models;

public class Bullet
{
    public required Position Target { get; set; }
    public required Position Source { get; set; }
    public required Tank Shooter { get; set; }
}
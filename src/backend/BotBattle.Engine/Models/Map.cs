namespace BotBattle.Engine.Models;

public class Map(int[,] tiles)
{
    public int[,] Tiles { get; set; } = tiles;

    public int[] Get1DArray()
    {
        return Tiles.Cast<int>().ToArray();
    }
}
namespace BotBattle.Engine.Models.MapGeneration;

public class Map(int[,] tiles)
{
    public int[,] Tiles { get; set; } = tiles;

    public int[] Get1DArray()
    {
        return Tiles.Cast<int>().ToArray();
    }
}
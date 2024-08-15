using BotBattle.Engine.Models.MapGeneration;

namespace BotBattle.Engine.Services.Map;

public class MapGeneratorService
{
    public static Models.MapGeneration.Map Generate(int width, int height)
    {
        var tiles = GenerateMapWithNoise(width, height, new List<TilesType>
        {
            //TilesType.Water,
            TilesType.Grass,
            TilesType.Sand
        });

        return new Models.MapGeneration.Map(tiles);
    }

    public static int[,] GenerateMapWithNoise(int width, int height, List<TilesType> tilesTypes)
    {
        var noiseMap = NoiseService.GenerateNoiseMap(width, height, tilesTypes.Count);

        var tiles = new int[height, width];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var noseValue = noiseMap.Noise[x, y];
            var index = noiseMap.GetSegmetIndex(noseValue);
            tiles[y, x] = (int)tilesTypes[index];
        }

        return tiles;
    }
}
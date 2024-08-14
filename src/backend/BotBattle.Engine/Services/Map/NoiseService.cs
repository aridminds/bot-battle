using BotBattle.Engine.Models.MapGeneration;
using SimplexNoise;

namespace BotBattle.Engine.Services.Map;

public static class NoiseService
{
    private const float Scale = 0.10f;

    public static NoiseMap GenerateNoiseMap(int width, int height, int segmentsCount)
    {
        var noise = GenerateNoise(width, height);
        var segments = CalculateSegments(noise, segmentsCount);
        return new NoiseMap(noise, segments);
    }

    public static List<NoiseSegment> GenerateSegments(int width, int height, int segmentsCount)
    {
        var noise = GenerateNoise(width, height);
        return CalculateSegments(noise, segmentsCount);
    }

    public static float[,] GenerateNoise(int width, int height)
    {
        return Noise.Calc2D(width, height, Scale); // Returns an array containing 2D Simplex noise
    }

    public static List<NoiseSegment> CalculateSegments(float[,] values, int segmentsCount)
    {
        var allValues = new List<float>();

        foreach (var value in values) allValues.Add(value);
        allValues.Sort();

        var count = allValues.Count;
        if (count == 0) throw new InvalidOperationException("The array is empty.");

        // Calculate the size of each segment
        var segmentSize = count / segmentsCount;
        var segments = new List<NoiseSegment>();

        for (var i = 0; i < segmentsCount; i++)
        {
            var start = i * segmentSize;
            var end = i == segmentsCount - 1 ? count : start + segmentSize;
            var segment = new NoiseSegment(allValues.GetRange(start, end - start));
            segments.Add(segment);
        }

        return segments;
    }
}
namespace BotBattle.Engine.Models.MapGeneration;

public readonly struct NoiseSegment(IEnumerable<float> values)
{
    public IEnumerable<float> Values { get; } = values;
    public float Min => Values.Min();
    public float Max => Values.Max();
}
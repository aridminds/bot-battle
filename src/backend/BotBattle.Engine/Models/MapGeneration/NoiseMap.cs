namespace BotBattle.Engine.Models.MapGeneration;

public class NoiseMap
{
    public NoiseMap(float[,] noise,  List<NoiseSegment> segments)
    {
        Noise = noise;
        Segments = segments;
    }

    public float[,] Noise { get; set; }
    public List<NoiseSegment> Segments { get; set; }

    public int GetSegmetIndex(float value)
    {
        for (int i = 0; i < Segments.Count; i++)
        {
            if(Segments[i].Min <= value && value <= Segments[i].Max)
            {
                return i;
            }
        }
        
        throw new IndexOutOfRangeException("Value is out of range.");
    }
}
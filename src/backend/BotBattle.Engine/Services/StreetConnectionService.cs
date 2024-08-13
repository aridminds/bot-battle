using BotBattle.Engine.MapGenerator.Models;

namespace BotBattle.Engine.MapGenerator.Services;

public class StreetConnectionService
{
    private Random _random;
    
    public StreetConnectionService(int? seed = null)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }
      
        
    public StreetTypes GetRandomConnection(StreetTypes currentStreetType)
    {
        var possibleConnections = GetPossibleConnections(currentStreetType);
        int index = _random.Next(possibleConnections.Count);
        return possibleConnections[index];
    }
    
    public List<StreetTypes> GetPossibleConnections(StreetTypes currentStreetType)
    {
        switch (currentStreetType)
        {
            case StreetTypes.StraightHorizontal:
            case StreetTypes.StraightVertical:
                return new List<StreetTypes> { StreetTypes.StraightHorizontal, StreetTypes.StraightVertical, StreetTypes.Cross };
            case StreetTypes.CornerTopLeft:
            case StreetTypes.CornerTopRight:
            case StreetTypes.CornerBottomLeft:
            case StreetTypes.CornerBottomRight:
                return new List<StreetTypes> { StreetTypes.StraightHorizontal, StreetTypes.StraightVertical, StreetTypes.CornerTopLeft, StreetTypes.CornerTopRight, StreetTypes.CornerBottomLeft, StreetTypes.CornerBottomRight };
            case StreetTypes.Cross:
                return new List<StreetTypes> { StreetTypes.StraightHorizontal, StreetTypes.StraightVertical, StreetTypes.CornerTopLeft, StreetTypes.CornerTopRight, StreetTypes.CornerBottomLeft, StreetTypes.CornerBottomRight, StreetTypes.Cross };
            default:
                return new List<StreetTypes> { StreetTypes.Ground };
        }
    }
}
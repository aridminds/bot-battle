using BotBattle.Core.Enums;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services;
using Xunit.Abstractions;

namespace BotBattle.Tests.Services;

public class AirTrafficControllerTest
{
    private BoardState _boardState = new BoardState(10, 10);
    
    private readonly ITestOutputHelper _output;

    public AirTrafficControllerTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void StartAirplane()
    {
        for (int i = 0; i < 20; i++)
        {
            AirTrafficController.MoveAirplane(_boardState);
            if(_boardState.Airplane.Position != null)
                _output.WriteLine($"IsFlying = {_boardState.Airplane.IsFlying}; ParachuteStatus: {_boardState.Airplane.ParachuteStatus}; Destination: {_boardState.Airplane.DroppingGiftPosition.X} CurrentPosition: {_boardState.Airplane.Position.X}" );
        }
        
        
        
        
    }
}
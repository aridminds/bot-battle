
using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services;
using Xunit.Abstractions;

namespace BotBattle.Tests.Services;

public class AirTrafficControllerTest
{
    private readonly ITestOutputHelper _output;

    public AirTrafficControllerTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void MoveAirplaneTest()
    {
        var boardState = new BoardState(10, 10)
        {
            Airplane =
            {
                Position = new Position(-1, 0, Direction.East),
                DroppingGiftPosition = new Position(2, 0, Direction.East),
                DroppingGiftType = CollectibleItemType.AdditionalBullet,
                IsFlying = true
            }
        };

        Assert.True(boardState.CollectibleItems.Count == 0);
        for (var i = 0; i < 12; i++)
        {
            AirTrafficController.MoveAirplane(boardState);
            _output.WriteLine($"IsFlying = {boardState.Airplane.IsFlying}; ParachuteStatus: {boardState.Airplane.ParachuteStatus}; Destination: {boardState.Airplane.DroppingGiftPosition?.X} CurrentPosition: {boardState.Airplane.Position?.X}" );
        }
        
        Assert.True(boardState.CollectibleItems.Count == 1);
    }
}
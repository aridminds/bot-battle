using BotBattle.Brain;
using BotBattle.Brain.Models;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;

namespace BotBattle.Tests.Services;

public class TankCalculatorTest
{
    [Theory]
    [InlineData(Direction.North, 5)]
    [InlineData(Direction.South, 5)]
    [InlineData(Direction.East, 5)]
    [InlineData(Direction.West, 5)]
    [InlineData(Direction.NorthEast, 5)]
    [InlineData(Direction.NorthWest, 5)]
    [InlineData(Direction.SouthEast, 5)]
    [InlineData(Direction.SouthWest, 5)]
    public void CalculateShootingRange_Square_Map_FullPower_CenterTank_Middle_Test(Direction direction, int expectedShootingRange)
    {
        //Arrange
        const int width = 10;
        const int height = 10;
        const int rawShootingRange = 255;
        const int blastRadius = 3;
        var tankPosition = new Position(5, 5)
        {
            Direction = direction
        };

        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 0)]
    [InlineData(Direction.South, 10)]
    [InlineData(Direction.East, 10)]
    [InlineData(Direction.West, 0)]
    [InlineData(Direction.NorthEast, 0)]
    [InlineData(Direction.NorthWest, 0)]
    [InlineData(Direction.SouthEast, 10)]
    [InlineData(Direction.SouthWest, 0)]
    public void CalculateShootingRange_Square_Map_FullPower_CenterTank_North_GroundZero_Test(Direction direction, int expectedShootingRange)
    {
        //Arrange
        const int width = 10;
        const int height = 10;
        const int rawShootingRange = 255;
        const int blastRadius = 3;
        var tankPosition = new Position(0, 0)
        {
            Direction = direction
        };

        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }

    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 5)]
    [InlineData(Direction.East, 255, 10)]
    [InlineData(Direction.West, 255, 10)]
    [InlineData(Direction.NorthEast, 255, 5)]
    [InlineData(Direction.NorthWest, 255, 5)]
    [InlineData(Direction.SouthEast, 255, 5)]
    [InlineData(Direction.SouthWest, 255, 5)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_CenterTank_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(10, 5)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 5)] 
    [InlineData(Direction.East, 255, 15)]
    [InlineData(Direction.West, 255, 5)]
     [InlineData(Direction.NorthEast, 255, 5)]
     [InlineData(Direction.NorthWest, 255, 5)]
     [InlineData(Direction.SouthEast, 255, 5)]
     [InlineData(Direction.SouthWest, 255, 5)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_EdgeTank_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(5, 5)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 5)] 
    [InlineData(Direction.East, 255, 20)]
    [InlineData(Direction.West, 255, 0)]
    [InlineData(Direction.NorthEast, 255, 5)]
     [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 5)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_GroundZero_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(0, 5)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 0)]
    [InlineData(Direction.South, 255, 10)] 
    [InlineData(Direction.East, 255, 20)]
    [InlineData(Direction.West, 255, 0)]
    [InlineData(Direction.NorthEast, 255, 0)]
    [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 10)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_North_GroundZero_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(0, 0)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 10)]
    [InlineData(Direction.South, 255, 0)] 
    [InlineData(Direction.East, 255, 20)]
    [InlineData(Direction.West, 255, 0)]
    [InlineData(Direction.NorthEast, 255, 10)]
    [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 0)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_South_GroundZero_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(0, 10)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 10)]
    [InlineData(Direction.South, 255, 0)] 
    [InlineData(Direction.East, 255, 0)]
    [InlineData(Direction.West, 255, 20)]
    [InlineData(Direction.NorthEast, 255, 0)]
    [InlineData(Direction.NorthWest, 255, 10)]
    [InlineData(Direction.SouthEast, 255, 0)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_South_GroundZeroWest_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(20, 10)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 0)]
    [InlineData(Direction.South, 255, 10)] 
    [InlineData(Direction.East, 255, 0)]
    [InlineData(Direction.West, 255, 20)]
    [InlineData(Direction.NorthEast, 255, 0)]
    [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 0)]
    [InlineData(Direction.SouthWest, 255, 10)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_North_GroundZeroWest_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        const int blastRadius = 3;
        var tankPosition = new Position(20, 0)
        {
            Direction = direction
        };
        //Act
        var result = TankCalculator.CalculateShootingRange(rawShootingRange, blastRadius, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
}
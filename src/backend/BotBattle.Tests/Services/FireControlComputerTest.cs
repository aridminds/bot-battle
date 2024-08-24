using BotBattle.Brain.Models;
using BotBattle.Engine.Services;

namespace BotBattle.Tests.Services;

public class FireControlComputerTest
{
    #region CalculateShootingRange
    [Theory]
    [InlineData(Direction.North, 5)]
    [InlineData(Direction.South, 4)]
    [InlineData(Direction.East, 4)]
    [InlineData(Direction.West, 5)]
    [InlineData(Direction.NorthEast, 4)]
    [InlineData(Direction.NorthWest, 5)]
    [InlineData(Direction.SouthEast, 4)]
    [InlineData(Direction.SouthWest, 4)]
    public void CalculateShootingRange_Square_Map_FullPower_CenterTank_Middle_Test(Direction direction, int expectedShootingRange)
    {
        //Arrange
        const int width = 10;
        const int height = 10;
        const int rawShootingRange = 255;
        var tankPosition = new Position(5, 5)
        {
            Direction = direction
        };

        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 0)]
    [InlineData(Direction.South, 9)]
    [InlineData(Direction.East, 9)]
    [InlineData(Direction.West, 0)]
    [InlineData(Direction.NorthEast, 0)]
    [InlineData(Direction.NorthWest, 0)]
    [InlineData(Direction.SouthEast, 9)]
    [InlineData(Direction.SouthWest, 0)]
    public void CalculateShootingRange_Square_Map_FullPower_CenterTank_North_GroundZero_Test(Direction direction, int expectedShootingRange)
    {
        //Arrange
        const int width = 10;
        const int height = 10;
        const int rawShootingRange = 255;
        
        var tankPosition = new Position(0, 0)
        {
            Direction = direction
        };

        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange, width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }

    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 4)]
    [InlineData(Direction.East, 255, 9)]
    [InlineData(Direction.West, 255, 10)]
    [InlineData(Direction.NorthEast, 255, 4)]
    [InlineData(Direction.NorthWest, 255, 5)]
    [InlineData(Direction.SouthEast, 255, 4)]
    [InlineData(Direction.SouthWest, 255, 4)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_CenterTank_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 20;
        const int height = 10;
        
        var tankPosition = new Position(10, 5)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 5)] 
    [InlineData(Direction.East, 255, 15)]
    [InlineData(Direction.West, 255, 5)]
     [InlineData(Direction.NorthEast, 255, 4)]
     [InlineData(Direction.NorthWest, 255, 5)]
     [InlineData(Direction.SouthEast, 255, 5)]
     [InlineData(Direction.SouthWest, 255, 5)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_EdgeTank_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(5, 5)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 5)]
    [InlineData(Direction.South, 255, 5)] 
    [InlineData(Direction.East, 255, 20)]
    [InlineData(Direction.West, 255, 0)]
    [InlineData(Direction.NorthEast, 255, 4)]
     [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 5)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_Middle_GroundZero_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(0, 5)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
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
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(0, 0)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 10)]
    [InlineData(Direction.South, 255, 0)] 
    [InlineData(Direction.East, 255, 20)]
    [InlineData(Direction.West, 255, 0)]
    [InlineData(Direction.NorthEast, 255, 9)]
    [InlineData(Direction.NorthWest, 255, 0)]
    [InlineData(Direction.SouthEast, 255, 0)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_South_GroundZero_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(0, 10)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    
    [Theory]
    [InlineData(Direction.North, 255, 10)]
    [InlineData(Direction.South, 255, 0)] 
    [InlineData(Direction.East, 255, 0)]
    [InlineData(Direction.West, 255, 20)]
    [InlineData(Direction.NorthEast, 255, 1)]
    [InlineData(Direction.NorthWest, 255, 10)]
    [InlineData(Direction.SouthEast, 255, 0)]
    [InlineData(Direction.SouthWest, 255, 0)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_South_GroundZeroWest_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(20, 10)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
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
    [InlineData(Direction.SouthEast, 255, 1)]
    [InlineData(Direction.SouthWest, 255, 10)]
    public void CalculateShootingRange_Rectangular_Map_FullPower_North_GroundZeroWest_Test(Direction direction, int rawShootingRange,
        int expectedShootingRange)
    {
        //Arrange
        const int width = 21;
        const int height = 11;
        
        var tankPosition = new Position(20, 0)
        {
            Direction = direction
        };
        //Act
        var result = FireControlComputer.CalculateShootingRange(rawShootingRange,  width, height, tankPosition);
        //Assert
        Assert.Equal(expectedShootingRange, result);
    }
    #endregion
    
    #region CalculateHealthReduction
    
    [Fact]
    public void CalculateHealthReduction_0_Test()
    {
        //Arrange
        const double distance = 0;
        //Act
        var result = FireControlComputer.CalculateHealthReduction(distance);
        //Assert
        Assert.Equal(30, result);
    }
    
    [Fact]
    public void CalculateHealthReduction_1_Test()
    {
        //Arrange
        const double distance = 1;
        //Act
        var result = FireControlComputer.CalculateHealthReduction(distance);
        //Assert
        Assert.Equal(20, result);
    }
    
    [Fact]
    public void CalculateHealthReduction_2_Test()
    {
        //Arrange
        const double distance = 2;
        //Act
        var result = FireControlComputer.CalculateHealthReduction(distance);
        //Assert
        Assert.Equal(10, result);
    }
    
    [Fact]
    public void CalculateHealthReduction_3_Test()
    {
        //Arrange
        const double distance = 3;
        //Act
        var result = FireControlComputer.CalculateHealthReduction(distance);
        //Assert
        Assert.Equal(0, result);
    }
    
    #endregion

    #region CalculateDistance

    [Fact]
    public void CalculateDistance_0_1_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(0,1);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void CalculateDistance_1_0_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(1,0);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void CalculateDistance_1_1_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(1,1);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void CalculateDistance_1_2_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(1,2);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(2, result);
    }
    
    [Fact]
    public void CalculateDistance_2_2_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(2,2);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(2, result);
    }
    
    [Fact]
    public void CalculateDistance_3_2_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(3,2);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(3, result);
    }
    
    [Fact]
    public void CalculateDistance_3_3_Test()
    {
        //Arrange
        var pos1 = new Position(0,0);
        var pos2 = new Position(3,3);
        //Act
        var result = FireControlComputer.CalculateDistance(pos1, pos2);
        //Assert
        Assert.Equal(3, result);
    }

    #endregion
}
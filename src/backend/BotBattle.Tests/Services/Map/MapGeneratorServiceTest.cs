using BotBattle.Engine.Services;
using Xunit.Abstractions;

namespace BotBattle.Tests.Services.Map;

public class MapGeneratorServiceTest
{
    private readonly ITestOutputHelper output;

    public MapGeneratorServiceTest(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    public void GenerateMapTest()
    {
        // Arrange
        var width = 10;
        var height = 10;
        
        // Act
        var map = MapGeneratorService.Generate(width, height);
        
        // Assert
        Assert.Equal(width, map.Tiles.GetLength(0));
        Assert.Equal(height, map.Tiles.GetLength(1));
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Assert.InRange(map.Tiles[y, x], 0, 2);
            }
        }
        
        //Show all on console
        for (var y = 0; y < height; y++)
        {
            var line = "";
            for (var x = 0; x < width; x++)
            {
                line += map.Tiles[y, x];
            }
            output.WriteLine(line);
        }
    }
    
    [Fact]
    public void GenerateMapTest_ThrowsException()
    {
        // Arrange
        var width = 0;
        var height = 0;
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MapGeneratorService.Generate(width, height));
    }
    
}
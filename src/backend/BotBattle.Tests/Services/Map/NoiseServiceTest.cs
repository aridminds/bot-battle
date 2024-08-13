using BotBattle.Engine.Services.Map;

namespace BotBattle.Tests.Services.Map;

public class NoiseServiceTest
{
    [Fact]
    public void GenerateNoiseTest()
    {
        // Arrange
        var width = 10;
        var height = 10;
        
        // Act
        var noise = NoiseService.GenerateNoise(width, height);
        
        // Assert
        Assert.Equal(width, noise.GetLength(0));
        Assert.Equal(height, noise.GetLength(1));
    }
    
    [Fact]
    public void CalculateSegmentsTest()
    {
        // Arrange
        var values = new float[10, 10];
        var x = 2;
        
        // Act
        var segments = NoiseService.CalculateSegments(values, x);
        
        // Assert
        Assert.Equal(x, segments.Count);
    }
    
    [Fact]
    public void CalculateSegmentsTest_ThrowsException()
    {
        // Arrange
        var values = new float[0, 0];
        var x = 2;
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => NoiseService.CalculateSegments(values, x));
    }
    
    [Fact]
    public void GenerateSegmentsTest()
    {
        // Arrange
        var width = 10;
        var height = 10;
        var x = 2;
        
        // Act
        var segments = NoiseService.GenerateSegments(width, height, x);
        
        // Assert
        Assert.Equal(x, segments.Count);
    }
    
    [Fact]
    public void GenerateNoiseMapTest()
    {
        // Arrange
        var width = 10;
        var height = 10;
        var x = 2;
        
        // Act
        var noiseMap = NoiseService.GenerateNoiseMap(width, height, x);
        
        // Assert
        Assert.Equal(width, noiseMap.Noise.GetLength(0));
        Assert.Equal(height, noiseMap.Noise.GetLength(1));
        Assert.Equal(x, noiseMap.Segments.Count);
    }
}
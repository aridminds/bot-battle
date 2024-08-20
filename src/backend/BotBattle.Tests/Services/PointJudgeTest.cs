using BotBattle.Engine.Models;
using BotBattle.Engine.Services;


namespace BotBattle.Tests.Services;

public class PointJudgeTest
{
    [Fact]
    public void CalculatePoints_FullHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank"
        };
        PointJudge.CalculatePoints(tank, 30, false, false);
        Assert.Equal(9, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_HalfHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name =  "TestTank"
        };
        PointJudge.CalculatePoints(tank, 20, false, false);
        Assert.Equal(6, tank.PointRegister);
    }
    
    [Fact]
    public void CalculatePoints_10PointsHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name =  "TestTank"
        };
        PointJudge.CalculatePoints(tank, 10, false, false);
        Assert.Equal(3, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_SelfHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name =  "TestTank"
        };
        PointJudge.CalculatePoints(tank, 30, true, false);
        Assert.Equal(-9, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_Kill()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name =  "TestTank"
        };
        PointJudge.CalculatePoints(tank, 30, false, true);
        Assert.Equal(18, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_SelfKill()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name =  "TestTank"
        };
        PointJudge.CalculatePoints(tank, 30, true, true);
        Assert.Equal(-18, tank.PointRegister);
    }
}

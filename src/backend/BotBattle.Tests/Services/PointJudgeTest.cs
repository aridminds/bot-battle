using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;

namespace BotBattle.Tests.Services;

public class PointJudgeTest
{
    private readonly BoardState boardState = new BoardState(10, 10);
    
    
    [Fact]
    public void CalculatePoints_FullHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank"
        };
        
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 30, false, false, boardState);
        Assert.Equal(9, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_HalfHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank"
        };
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 20, false, false, boardState);
        Assert.Equal(6, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_10PointsHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank"
        };
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 10, false, false, boardState);
        Assert.Equal(3, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_SelfHit()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 10,
            Name = "TestTank"
        };
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 30, true, false, boardState);
        Assert.Equal(1, tank.PointRegister);
    }

    [Fact]
    public void CalculatePoints_Kill()
    {
        var tank1 = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank1"
        };
        
        var tank2 = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank2",
            Status = TankStatus.Dead,
            DiedInTurn = 0
            
        };
        
        var tank3 = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank3",
            Status = TankStatus.Dead,
            DiedInTurn = 1
            
        };
        boardState.Turns = 3;
        boardState.Tanks.Add(tank1);
        boardState.Tanks.Add(tank2);
        boardState.Tanks.Add(tank3);
        
        PointJudge.CalculatePoints(tank1, 30, false, true, boardState);
        Assert.Equal(38, tank1.PointRegister);
    }

    [Fact]
    public void CalculatePoints_SelfKill()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 20,
            Name = "TestTank",
        };
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 30, true, true, boardState);
        Assert.Equal(2, tank.PointRegister);
    }
    
    [Fact]
    public void CalculatePoints_SelfKill_ToZero()
    {
        var tank = new Tank
        {
            Health = 100,
            PointRegister = 0,
            Name = "TestTank",
        };
        boardState.Tanks.Add(tank);

        PointJudge.CalculatePoints(tank, 30, true, true, boardState);
        Assert.Equal(0, tank.PointRegister);
    }
}
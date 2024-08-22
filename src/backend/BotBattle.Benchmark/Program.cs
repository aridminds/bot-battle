using System.Diagnostics;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using BotBattle.Brain.Models;
using BotBattle.Engine;
using BotBattle.Engine.Helper;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;
using BenchmarkDotNet.Diagnostics.dotMemory;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace BotBattle.Benchmark;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Benchmarks));
    }
}

[MemoryDiagnoser]
public class Benchmarks
{
    private Tank _currentTank;
    private readonly GameMaster _gameMaster = new();
    private BoardState _boardState;

   
     [Params(10, 100, 1000)]
    public int width;
    
    [Params(10, 100, 1000)]
    public int height;
    
    [Params(1000, 2000, 3000)]
    public int rounds;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _boardState = new BoardState(width,height);
        
        for (int i = 0; i < 10; i++)
        {
            _boardState.Tanks.Add(new Tank {Name = $"Tank{i+1}"});
        }
        foreach (var tank in _boardState.Tanks)
        {
            tank.Position = StartPositionService.SetStartPosition(_boardState);
        }
        for (var i = 0; i < 20; i++)
        {
            _boardState.Obstacles.Add(new Obstacle
            {
                Position = StartPositionService.SetStartPosition(_boardState),
                Type = EnumHelper.GetRandomEnumValue<ObstacleType>(ObstacleType.Destroyed)
            });
        }
        
        _currentTank = _boardState.Tanks.First();
    }

    [Benchmark]
    public void RunGame()
    {
        for (int i = 0; i < rounds; i++)
        {
            if(_boardState.Status == GameStatus.GameOver) break;
            if (_currentTank.Status == TankStatus.Alive)
            {
                var data = CallDataSource();
                var payloadHash = MD5.HashData(BitConverter.GetBytes(data.Item1 * data.Item2));
                var payloadHashString = BitConverter.ToString(payloadHash).Replace("-", "");

                _boardState = _gameMaster.NextRound(payloadHashString, _boardState, _currentTank);
            }

            NextPlayer();
        }
    }

    private void NextPlayer()
    {
        var currentPlayerIndex = _boardState.Tanks.IndexOf(_currentTank);

        var nextPlayerIndex = (currentPlayerIndex + 1) % _boardState.Tanks.Count;
        _currentTank = _boardState.Tanks[nextPlayerIndex];
    }
    
    private (long, long) CallDataSource()
    {
        return (new Random().Next(0, 5000), new Random().Next(0, 5000));
    }

}

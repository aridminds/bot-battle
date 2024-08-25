using BotBattle.AgentLib;
using BotBattle.AgentLib.Enums;
using BotBattle.AgentRunner;
using BotBattle.Core;
using Bullet = BotBattle.AgentLib.Bullet;
using Direction = BotBattle.AgentLib.Enums.Direction;
using Obstacle = BotBattle.AgentLib.Obstacle;
using Position = BotBattle.AgentLib.Position;
using Tank = BotBattle.AgentLib.Tank;

namespace BotBattle.Tests;

public class WasmRunnerTests
{
    [Fact]
    public void TestRun()
    {
        var wasmFile = File.ReadAllBytes("Resources/BotBattle.Agent.wasm");

        var tanks = new List<Tank>
        {
            new Tank
            {
                Position = new Position { X = 1, Y = 2 },
                Direction = Direction.North,
                Health = 100
            }
        };

        var bullets = new List<Bullet>
        {
            new()
            {
                Owner = tanks[0],
                Position = new Position { X = 1, Y = 2 },
                Direction = Direction.West
            }
        };

        var obstacles = new List<Obstacle>
        {
            new()
            {
                Position = new Position { X = 1, Y = 2 }, Type = ObstacleType.Destroyed, Direction = Direction.North
            }
        };

        var arena = new Arena
        {
            Width = 10,
            Height = 10,
            Turn = 0,
            Tanks = tanks,
            Bullets = bullets,
            Obstacles = obstacles
        };

        var players = new List<Player>
        {
            new()
            {
                Name = "player1",
                Code = wasmFile
            }
        };

        var wasmRunner = new WasmRunner(players.ToArray());
        var response = wasmRunner.Execute("player1", arena);

        Assert.NotNull(response);
    }
}
using BotBattle.AgentLib;
using BotBattle.AgentLib.Enums;
using BotBattle.AgentRunner;
using BotBattle.Core;
using Xunit.Abstractions;
using Bullet = BotBattle.AgentLib.Bullet;
using Direction = BotBattle.AgentLib.Enums.Direction;
using Obstacle = BotBattle.AgentLib.Obstacle;
using ObstacleType = BotBattle.AgentLib.Enums.ObstacleType;
using Position = BotBattle.AgentLib.Position;
using Rotate = BotBattle.AgentLib.Rotate;
using Shoot = BotBattle.AgentLib.Shoot;
using Drive = BotBattle.AgentLib.Drive;
using Tank = BotBattle.AgentLib.Tank;
using WeaponSystem = BotBattle.AgentLib.WeaponSystem;

namespace BotBattle.Tests;

public class WasmRunnerTests
{
    private readonly ITestOutputHelper _output;

    public WasmRunnerTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
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
                Health = 1000,
                Name = "player1",
                WeaponSystem = new WeaponSystem()
                {
                    Bullet = BulletType.Standard,
                    Id = Guid.Empty,
                    FireCooldown = (float)0.0,
                    ActiveFireCooldown = (float)0.0,
                    
                }
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
              PathToWasm  = @"C:\Users\Matth\source\repos\AridMinds\bot-battle\sample\botbattle_agent_rust\target\wasm32-unknown-unknown\release\botbattle_agent_rust.wasm",
                
            }
        };

        var wasmRunner = new WasmRunner(players.ToArray());
        var response = wasmRunner.Execute("player1", arena);

        Assert.NotNull(response);

        if (response.Action is Shoot shoot)
        {
            _output.WriteLine($"Shoot: {shoot.Power}");
        } else if (response.Action is Drive drive)
        {
            _output.WriteLine("Drive");
        }
        else if(response.Action is Rotate rotate)
        {
             _output.WriteLine("Rotate " + rotate.Direction);
        }
    }
}
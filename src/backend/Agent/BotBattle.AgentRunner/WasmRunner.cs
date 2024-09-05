using System.Security.Cryptography;
using BotBattle.AgentLib;
using BotBattle.Core;
using Extism.Sdk;

namespace BotBattle.AgentRunner;

public class WasmRunner
{
    private const string EntryPoint = "calculate_action";
    private readonly Dictionary<string, Plugin> _plugins = new();

    public WasmRunner(params Player[] players)
    {
        foreach (var player in players)
        {
            var manifest = new Manifest(new PathWasmSource(player.PathToWasm));
            _plugins[player.Name] = new Plugin(manifest, [], withWasi: true);
        }
    }

    public AgentResponse? Execute(string player, Arena arena)
    {
        var plugin = _plugins[player];
        return plugin.Call(EntryPoint,
            new AgentRequest
            {
                Arena = arena,
                Hash = MD5.HashData(BitConverter.GetBytes(new Random().Next(0, 5000) * new Random().Next(0, 5000))),
                MyTank = arena.Tanks.First(t => t.Name == player)
            },
            AgentJsonContext.Default.AgentRequest,
            AgentJsonContext.Default.AgentResponse
        );
    }
}
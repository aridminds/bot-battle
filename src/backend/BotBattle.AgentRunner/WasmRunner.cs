using BotBattle.AgentLib;
using BotBattle.Core;
using Extism.Sdk;

namespace BotBattle.AgentRunner;

public class WasmRunner
{
    private const string EntryPoint = "calculate_action";
    private readonly Manifest _manifest;
    private readonly Dictionary<string, Plugin> _plugins = new();

    public WasmRunner(params Player[] players)
    {
        _manifest = new Manifest(players.Select(p => new ByteArrayWasmSource(p.Code, p.Name)).ToArray<WasmSource>())
        {
            Timeout = TimeSpan.FromMilliseconds(1000)
        };

        foreach (var player in players)
        {
            _plugins[player.Name] = new Plugin(_manifest, [], withWasi: true);
        }
    }

    public AgentResponse? Execute(string player, Arena arena)
    {
        var plugin = _plugins[player];
        return plugin.Call(EntryPoint,
            new AgentRequest { Arena = arena },
            AgentJsonContext.Default.AgentRequest,
            AgentJsonContext.Default.AgentResponse
        );
    }
}
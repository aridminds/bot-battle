using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Extism;

namespace BotBattle.Agent;

[JsonSerializable(typeof(object))]
public partial class SourceGenerationContext : JsonSerializerContext
{
}

public static class Program
{
    public static void Main()
    {
        // Note: a `Main` method is required for the app to compile
    }

    [UnmanagedCallersOnly]
    public static int add()
    {
        var parameters = Pdk.GetInputJson(global::SourceGenerationContext.Default.Add);
        var sum = new Sum(parameters.a + parameters.b);
        Pdk.SetOutputJson(sum, global::SourceGenerationContext.Default.Sum);
        return 0;
    }
}
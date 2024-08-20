using System.Threading.Channels;

namespace BotBattle.Api.Services.Broadcast;

public interface ISubscription<S> : IDisposable
{
    ChannelReader<S> Reader { get; }
}

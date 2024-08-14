using System.Collections.Immutable;
using System.Threading.Channels;

namespace BotBattle.Api.Services;

public interface ISubscription<S> : IDisposable
{
    ChannelReader<S> Reader { get; }
}

public class BroadcastService<T>
{
    private readonly object _lock = new();

    private ImmutableList<Subscription<T>> _subscriptions = [];

    private void RemoveSubscription(Subscription<T> sub)
    {
        lock (_lock)
        {
            _subscriptions = _subscriptions.Remove(sub);
        }
    }

    private void AddSubscription(Subscription<T> sub)
    {
        lock (_lock)
        {
            _subscriptions = _subscriptions.Add(sub);
        }
    }

    public async Task Broadcast(ChannelReader<T> source, CancellationToken ct = default)
    {
        await foreach (var msg in source.ReadAllAsync(ct).ConfigureAwait(false))
        {
            var subCopy = new List<Subscription<T>>(_subscriptions);
            foreach (var item in subCopy) await item.channel.Writer.WriteAsync(msg, ct);
        }
    }

    public ISubscription<T> RegisterOutboundChannel()
    {
        return new Subscription<T>(AddSubscription, RemoveSubscription, Channel.CreateUnbounded<T>());
    }

    public sealed class Subscription<S> : ISubscription<S>
    {
        public readonly Channel<S> channel;
        private readonly Action<Subscription<S>> unregister;
        private bool _isDisposed;

        public Subscription(Action<Subscription<S>> register, Action<Subscription<S>> unregister, Channel<S> channel)
        {
            register(this);
            this.unregister = unregister;
            this.channel = channel;
        }

        public ChannelReader<S> Reader => channel.Reader;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            unregister(this);
            channel.Writer.Complete();
        }
    }
}
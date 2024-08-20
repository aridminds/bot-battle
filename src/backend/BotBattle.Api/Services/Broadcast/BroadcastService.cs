using System.Collections.Immutable;
using System.Threading.Channels;

namespace BotBattle.Api.Services.Broadcast;

public class BroadcastService<T>
{
    private readonly object _lock = new();

    private ImmutableSortedDictionary<int, ImmutableSortedSet<Subscription<T>>> _subscriptions = ImmutableSortedDictionary<int, ImmutableSortedSet<Subscription<T>>>.Empty;

    private void RemoveSubscription(Subscription<T> sub, params int[] groups)
    {
        var tmpSubs = _subscriptions;
        var curatedGroups = groups.Where(tmpSubs.ContainsKey).ToArray();
        if (curatedGroups.Length == 0) return;

        lock (_lock)
        {
            var builder = _subscriptions.ToBuilder();
            foreach (var group in curatedGroups)
            {
                if (!builder.TryGetValue(group, out var subscriptions)) continue;
                builder[group] = subscriptions.Remove(sub);
            }
            _subscriptions = builder.ToImmutable();
        }
    }

    private void AddSubscription(Subscription<T> sub, params int[] groups)
    {
        lock (_lock)
        {
            var builder = _subscriptions.ToBuilder();
            foreach (var group in groups.Distinct())
            {
                if (!builder.TryGetValue(group, out var subscriptions))
                    subscriptions = builder[group] = [];
                builder[group] = subscriptions.Add(sub);
            }
            _subscriptions = builder.ToImmutable();
        }
    }

    public async Task Broadcast(ChannelReader<T> source, int group, CancellationToken ct = default)
    {
        await foreach (var msg in source.ReadAllAsync(ct).ConfigureAwait(false))
        {
            if (!_subscriptions.TryGetValue(group, out var subscriptions)) continue;
            foreach (var item in subscriptions) await item.channel.Writer.WriteAsync(msg, ct);
        }
    }

    /// <summary>
    /// Registers a subscription to the given <paramref name="groups"/>.
    /// Calling this without any groups results in an <see cref="ArgumentOutOfRangeException"/>.
    /// </summary>
    /// <param name="groups">The groups to register to.</param>
    /// <returns>A disposable subscription.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="groups"/> is empty.</exception>
    public ISubscription<T> RegisterOutboundChannel(params int[] groups)
    {
        ArgumentOutOfRangeException.ThrowIfZero(groups.Length, nameof(groups));
        return new Subscription<T>(AddSubscription, RemoveSubscription, groups, Channel.CreateUnbounded<T>());
    }

    public sealed class Subscription<S> : ISubscription<S>
    {
        public readonly Channel<S> channel;
        private readonly Action<Subscription<S>, int[]> unregister;
        private bool _isDisposed;
        private HashSet<int> _groups;

        public Subscription(Action<Subscription<S>, int[]> register, Action<Subscription<S>, int[]> unregister, int[] groups, Channel<S> channel)
        {
            register(this, groups);
            this.unregister = unregister;
            this.channel = channel;
            _groups = new(groups);
        }

        public ChannelReader<S> Reader => channel.Reader;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            unregister(this, [.. _groups]);
            channel.Writer.Complete();
        }
    }
}
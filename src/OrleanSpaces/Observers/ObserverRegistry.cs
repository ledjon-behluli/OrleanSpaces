using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry
{
    private readonly ConcurrentDictionary<DynamicObserver, Guid> observers = new();
    public IEnumerable<DynamicObserver> Observers => observers.Keys;

    public Guid Add(DynamicObserver observer)
    {
        if (!observers.TryGetValue(observer, out _))
        {
            observers.TryAdd(observer, Guid.NewGuid());
        }

        return observers[observer];
    }

    public void Remove(DynamicObserver observer)
        => observers.TryRemove(observer, out _);
}
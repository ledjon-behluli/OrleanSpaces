using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry
{
    private readonly ConcurrentDictionary<SpaceObserver, Guid> observers = new();
    public IEnumerable<SpaceObserver> Observers => observers.Keys;

    public Guid Add(SpaceObserver observer)
    {
        if (!observers.TryGetValue(observer, out _))
        {
            observers.TryAdd(observer, Guid.NewGuid());
        }

        return observers[observer];
    }

    public void Remove(SpaceObserver observer)
        => observers.TryRemove(observer, out _);
}
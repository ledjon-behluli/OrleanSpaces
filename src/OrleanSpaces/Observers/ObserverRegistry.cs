using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry
{
    private readonly ConcurrentDictionary<ISpaceObserver, Guid> observers = new();
    public IEnumerable<ISpaceObserver> Observers => observers.Keys;

    public Guid Add(ISpaceObserver observer)
    {
        if (!observers.TryGetValue(observer, out _))
        {
            observers.TryAdd(observer, Guid.NewGuid());
        }

        return observers[observer];
    }

    public void Remove(ISpaceObserver observer)
        => observers.TryRemove(observer, out _);
}
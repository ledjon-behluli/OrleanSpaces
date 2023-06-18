using OrleanSpaces.Tuples;
using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry<T>
    where T : ISpaceTuple
{
    private readonly ConcurrentDictionary<SpaceObserver<T>, Guid> observers = new();
    public IEnumerable<SpaceObserver<T>> Observers => observers.Keys;

    public Guid Add(ISpaceObserver<T> observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        SpaceObserver<T> _observer = observer is SpaceObserver<T> spaceObserver ?
            spaceObserver : new ObserverDecorator<T>(observer);

        if (!observers.TryGetValue(_observer, out _))
        {
            observers.TryAdd(_observer, Guid.NewGuid());
        }

        return observers[_observer];
    }

    public void Remove(Guid id)
    {
        SpaceObserver<T> key = observers.SingleOrDefault(x => x.Value == id).Key;
        if (key != null)
        {
            observers.TryRemove(key, out _);
        }
    }
}
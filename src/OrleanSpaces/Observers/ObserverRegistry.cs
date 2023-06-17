using OrleanSpaces.Tuples;
using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly ConcurrentDictionary<SpaceObserver<TTuple, TTemplate>, Guid> observers = new();
    public IEnumerable<SpaceObserver<TTuple, TTemplate>> Observers => observers.Keys;

    public Guid Add(ISpaceObserver<TTuple, TTemplate> observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        SpaceObserver<TTuple, TTemplate> _observer = 
            observer is SpaceObserver<TTuple, TTemplate> spaceObserver ?
            spaceObserver : new ObserverDecorator<TTuple, TTemplate>(observer);

        if (!observers.TryGetValue(_observer, out _))
        {
            observers.TryAdd(_observer, Guid.NewGuid());
        }

        return observers[_observer];
    }

    public void Remove(Guid id)
    {
        SpaceObserver<TTuple, TTemplate> key = observers.SingleOrDefault(x => x.Value == id).Key;
        if (key != null)
        {
            observers.TryRemove(key, out _);
        }
    }
}
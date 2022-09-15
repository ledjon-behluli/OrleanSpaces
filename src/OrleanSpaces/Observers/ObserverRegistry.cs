using OrleanSpaces.Primitives;
using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry
{
    private readonly ConcurrentDictionary<SpaceObserver, Guid> observers = new();
    public IEnumerable<SpaceObserver> Observers => observers.Keys;

    public Guid Add(ISpaceObserver observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        SpaceObserver _observer = observer is SpaceObserver spaceObserver ?
            spaceObserver : new ObserverDecorator(observer);

        if (!observers.TryGetValue(_observer, out _))
        {
            observers.TryAdd(_observer, Guid.NewGuid());
        }

        return observers[_observer];
    }

    public void Remove(Guid id)
    {
        SpaceObserver key = observers.SingleOrDefault(x => x.Value == id).Key;
        if (key != null)
        {
            observers.TryRemove(key, out _);
        }
    }

    private class ObserverDecorator : SpaceObserver
    {
        private readonly ISpaceObserver observer;

        public ObserverDecorator(ISpaceObserver observer)
        {
            this.observer = observer;
            ListenTo(Everything);
        }

        public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
            await observer.OnExpansionAsync(tuple, cancellationToken);

        public override async Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) =>
            await observer.OnContractionAsync(template, cancellationToken);

        public override async Task OnFlatteningAsync(CancellationToken cancellationToken) =>
            await observer.OnFlatteningAsync(cancellationToken);
    }
}
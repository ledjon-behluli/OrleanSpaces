using OrleanSpaces.Primitives;
using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal sealed class ObserverRegistry
{
    private readonly ConcurrentDictionary<SpaceObserver, Guid> observers = new();
    public IEnumerable<SpaceObserver> Observers => observers.Keys;

    public Guid Add(ISpaceObserver observer)
    {
        SpaceObserver _observer = Compose(observer);

        if (!observers.TryGetValue(_observer, out _))
        {
            observers.TryAdd(_observer, Guid.NewGuid());
        }

        return observers[_observer];
    }

    public void Remove(ISpaceObserver observer)
        => observers.TryRemove(Compose(observer), out _);

    private static SpaceObserver Compose(ISpaceObserver observer) =>
        observer is SpaceObserver spaceObserver ?
        spaceObserver : new ObserverDecorator(observer);

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
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverDecorator<T> : SpaceObserver<T>
    where T : ISpaceTuple
{
    private readonly ISpaceObserver<T> observer;

    public ObserverDecorator(ISpaceObserver<T> observer)
    {
        this.observer = observer;
        ListenTo(Everything);
    }

    public override Task OnExpansionAsync(T tuple, CancellationToken cancellationToken) =>
        observer.OnExpansionAsync(tuple, cancellationToken);

    public override Task OnContractionAsync(T tuple, CancellationToken cancellationToken) =>
        observer.OnContractionAsync(tuple, cancellationToken);

    public override Task OnFlatteningAsync(CancellationToken cancellationToken) =>
        observer.OnFlatteningAsync(cancellationToken);
}
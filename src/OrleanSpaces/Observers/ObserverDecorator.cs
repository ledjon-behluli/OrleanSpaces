using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverDecorator : SpaceObserver
{
    private readonly ISpaceObserver observer;

    public ObserverDecorator(ISpaceObserver observer)
    {
        this.observer = observer;
        ListenTo(Everything);
    }

    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
        observer.OnExpansionAsync(tuple, cancellationToken);

    public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) =>
        observer.OnContractionAsync(template, cancellationToken);

    public override Task OnFlatteningAsync(CancellationToken cancellationToken) =>
        observer.OnFlatteningAsync(cancellationToken);
}
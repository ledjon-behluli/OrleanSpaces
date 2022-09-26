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

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
        await observer.OnExpansionAsync(tuple, cancellationToken);

    public override async Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) =>
        await observer.OnContractionAsync(template, cancellationToken);

    public override async Task OnFlatteningAsync(CancellationToken cancellationToken) =>
        await observer.OnFlatteningAsync(cancellationToken);
}
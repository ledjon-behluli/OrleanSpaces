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

internal sealed class ObserverDecorator<T, TTuple, TTemplate> : SpaceObserver<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private readonly ISpaceObserver<T, TTuple, TTemplate> observer;

    public ObserverDecorator(ISpaceObserver<T, TTuple, TTemplate> observer)
    {
        this.observer = observer;
        ListenTo(Everything);
    }

    public override Task OnExpansionAsync(TTuple tuple, CancellationToken cancellationToken) =>
        observer.OnExpansionAsync(tuple, cancellationToken);

    public override Task OnContractionAsync(TTemplate template, CancellationToken cancellationToken) =>
        observer.OnContractionAsync(template, cancellationToken);

    public override Task OnFlatteningAsync(CancellationToken cancellationToken) =>
        observer.OnFlatteningAsync(cancellationToken);
}
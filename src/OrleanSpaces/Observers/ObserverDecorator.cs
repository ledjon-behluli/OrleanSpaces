using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverDecorator<TTuple, TTemplate> : SpaceObserver<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly ISpaceObserver<TTuple, TTemplate> observer;

    public ObserverDecorator(ISpaceObserver<TTuple, TTemplate> observer)
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
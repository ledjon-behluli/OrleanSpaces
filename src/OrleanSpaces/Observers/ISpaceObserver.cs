using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken = default);
    Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken = default);
    Task OnFlatteningAsync(CancellationToken cancellationToken = default);
}

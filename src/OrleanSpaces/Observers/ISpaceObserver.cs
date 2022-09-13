using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken);
    Task OnRemovedAsync(SpaceTemplate template, CancellationToken cancellationToken);
    Task OnEmptyAsync(CancellationToken cancellationToken);
}
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    Task OnNewTupleAsync(SpaceTuple tuple, CancellationToken cancellationToken);
    Task OnEmptySpaceAsync(CancellationToken cancellationToken);
}
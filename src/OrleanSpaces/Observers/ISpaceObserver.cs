using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    // TODO: Look
    //Task OnSpaceExpansionAsync(SpaceTuple tuple, uint tuplesInSpace);
    //Task OnSpaceContractionAsync(SpaceTuple tuple, uint tuplesInSpace);

    Task OnTupleAsync(SpaceTuple tuple, CancellationToken cancellationToken);
    Task OnEmptySpaceAsync(CancellationToken cancellationToken);
}
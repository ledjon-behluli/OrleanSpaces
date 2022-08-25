using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    Task OnTupleAsync(SpaceTuple tuple);
}
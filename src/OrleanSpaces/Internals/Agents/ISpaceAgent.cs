using Orleans;
using OrleanSpaces.Types;

namespace OrleanSpaces.Internals.Agents;

public interface ISpaceAgent : IGrainObserver
{
    void OnTuple(SpaceTuple tuple);
}
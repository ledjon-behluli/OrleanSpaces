using Orleans;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Internals.Agents;

public interface ISpaceAgent : IGrainObserver
{
    void OnTuple(SpaceTuple tuple);
}
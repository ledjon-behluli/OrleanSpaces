using Orleans;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Core.Observers;

public interface ISpaceObserver : IGrainObserver
{
    void Receive(SpaceTuple tuple);
}
using Orleans;

namespace OrleanSpaces.Core;

public interface ISpaceObserver : IGrainObserver
{
    void Receive(SpaceTuple tuple);
}
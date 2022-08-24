using Orleans;
using Orleans.Concurrency;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Core.Observers;

public interface ISpaceObserver : IGrainObserver
{
    [OneWay]
    Task Receive(SpaceTuple tuple);
}
using Orleans;
using Orleans.Concurrency;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver : IGrainObserver
{
    [OneWay]
    Task ReceiveAsync(SpaceTuple tuple);
}
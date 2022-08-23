using Orleans;
using OrleanSpaces.Core.Observers;

namespace OrleanSpaces.Core;

internal static class Extensions
{
    public static IObserverRegistry GetObserverRegistry(this IGrainFactory factory)
        => factory.GetSpaceGrain();

    public static ISpaceGrain GetSpaceGrain(this IGrainFactory factory) 
        => factory.GetGrain<ISpaceGrain>(Guid.Empty);
}
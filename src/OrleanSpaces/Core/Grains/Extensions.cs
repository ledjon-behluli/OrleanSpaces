using Orleans;

namespace OrleanSpaces.Core.Grains;

internal static class Extensions
{
    public static ISpaceGrain GetSpaceGrain(this IGrainFactory factory) => factory.GetGrain<ISpaceGrain>(Guid.Empty);
}
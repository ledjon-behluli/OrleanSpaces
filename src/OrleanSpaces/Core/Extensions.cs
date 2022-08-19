using Orleans;

namespace OrleanSpaces.Core;

public static class Extensions
{
    public static ISpaceWriter GetSpaceWriter(this IGrainFactory factory)
        => factory.GetGrain<ISpaceGrain>(Guid.Empty);

    public static ISpaceReader GetSpaceReader(this IGrainFactory factory)
        => factory.GetGrain<ISpaceGrain>(Guid.Empty);

    public static ISpaceBlockingReader GetSpaceBlockingReader(this IGrainFactory factory)
        => factory.GetGrain<ISpaceGrain>(Guid.Empty);
}
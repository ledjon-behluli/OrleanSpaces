using Orleans;
using OrleanSpaces.Primitives;

namespace OrleanSpaces;

internal interface ISpaceGrain : IGrainWithGuidKey
{
    ValueTask<Guid> ListenAsync();

    Task WriteAsync(SpaceTuple tuple);
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);
    ValueTask<SpaceTuple> PopAsync(SpaceTemplate template);
    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);
    ValueTask<int> CountAsync(SpaceTemplate? template);
}

using Orleans;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : IGrainWithGuidKey
{
    ValueTask<Guid> ConnectAsync();
    Task WriteAsync(SpaceTuple tuple);
    Task EvaluateAsync(byte[] serializedFunc);
    ValueTask<SpaceTuple> PeekAsync(SpaceTemplate template);
    Task<SpaceTuple> PopAsync(SpaceTemplate template);
    ValueTask<IEnumerable<SpaceTuple>> ScanAsync(SpaceTemplate template);
    ValueTask<int> CountAsync(SpaceTemplate? template);
}

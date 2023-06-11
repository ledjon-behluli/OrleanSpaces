using Orleans;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

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

internal interface IIntGrain : IGrainWithGuidKey
{
    ValueTask<Guid> ListenAsync();
    Task AddAsync(IntTuple tuple);
    Task RemoveAsync(IntTuple tuple);
    ValueTask<IEnumerable<IntTuple>> GetAsync(IntTemplate template);
}

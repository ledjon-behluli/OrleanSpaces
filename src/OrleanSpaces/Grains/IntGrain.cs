using Orleans;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : IGrainWithGuidKey
{
    Task AddAsync(IntTuple tuple);
    Task RemoveAsync(IntTuple tuple);
    ValueTask<List<IntTuple>> GetAsync(IntTemplate template);
}

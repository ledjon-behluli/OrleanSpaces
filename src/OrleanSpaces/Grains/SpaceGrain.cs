using Orleans.Runtime;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : ITupleStore<SpaceTuple>, IGrainWithStringKey 
{
    const string Key = "SpaceStore";
}

internal sealed class SpaceGrain : BaseGrain<SpaceTuple>, ISpaceGrain
{
    public SpaceGrain(
        [PersistentState(ISpaceGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<SpaceTuple>> space) : base(ISpaceGrain.Key, space) { }
}
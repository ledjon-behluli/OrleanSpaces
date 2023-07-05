using Orleans.Runtime;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Grains;

internal interface ISpaceGrain : ITupleStore<SpaceTuple>, IGrainWithStringKey 
{
    const string Key = "SpaceStore";
}

internal sealed class SpaceGrain : Grain<SpaceTuple>, ISpaceGrain
{
    public SpaceGrain(
        [PersistentState(ISpaceGrain.Key, Constants.StorageName)]
        IPersistentState<List<SpaceTuple>> space) : base(ISpaceGrain.Key, space) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Directors;

internal interface ISpaceDirector : IStoreDirector<SpaceTuple>, IGrainWithStringKey
{
    const string Key = "SpaceDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceDirector : BaseDirector<SpaceTuple, ISpaceGrain>, ISpaceDirector
{
    public SpaceDirector(
        [PersistentState(ISpaceDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISpaceGrain.Key, storeIds) { }
}

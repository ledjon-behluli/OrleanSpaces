using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Grains.Directors;

internal interface ISpaceDirector : IStoreDirector<SpaceTuple>, IGrainWithStringKey
{
    const string Key = "SpaceDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceDirector : BaseDirector<SpaceTuple, ISpaceStore>, ISpaceDirector
{
    public SpaceDirector(
        [PersistentState(ISpaceDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISpaceStore.Key, storeIds) { }
}

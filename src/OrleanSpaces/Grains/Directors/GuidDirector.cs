using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IGuidDirector : IStoreDirector<GuidTuple>, IGrainWithStringKey
{
    const string Key = "GuidDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidDirector : BaseDirector<GuidTuple, IGuidStore>, IGuidDirector
{
    public GuidDirector(
        [PersistentState(IGuidDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IGuidStore.Key, storeIds) { }
}
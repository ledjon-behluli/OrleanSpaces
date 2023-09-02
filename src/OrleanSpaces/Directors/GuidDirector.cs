using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IGuidDirector : IStoreDirector<GuidTuple>, IGrainWithStringKey
{
    const string Key = "GuidDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidDirector : BaseDirector<GuidTuple, IGuidGrain>, IGuidDirector
{
    public GuidDirector(
        [PersistentState(IGuidDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IGuidGrain.Key, storeIds) { }
}
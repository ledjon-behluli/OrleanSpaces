using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface ISByteDirector : IStoreDirector<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteDirector : BaseDirector<SByteTuple, ISByteGrain>, ISByteDirector
{
    public SByteDirector(
        [PersistentState(ISByteDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISByteGrain.Key, storeIds) { }
}

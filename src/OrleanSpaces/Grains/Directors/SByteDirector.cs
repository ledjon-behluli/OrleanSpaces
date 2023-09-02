using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ISByteDirector : IStoreDirector<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteDirector : BaseDirector<SByteTuple, ISByteStore>, ISByteDirector
{
    public SByteDirector(
        [PersistentState(ISByteDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISByteStore.Key, storeIds) { }
}

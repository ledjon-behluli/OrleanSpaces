using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ISByteDirector : IStoreDirector<SByteTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteDirector : BaseDirector<SByteTuple, ISByteStore>, ISByteDirector
{
    public SByteDirector(
        [PersistentState(Constants.RealmKey_SByte, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_SByte, storeFullKeys) { }
}

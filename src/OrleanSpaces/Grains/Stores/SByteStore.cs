using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface ISByteStore : ITupleStore<SByteTuple>, IGrainWithStringKey { }

internal sealed class SByteStore : BaseStore<SByteTuple>, ISByteStore
{
    public SByteStore(
        [PersistentState(Constants.RealmKey_SByte, Constants.StorageName)]
        IPersistentState<List<SByteTuple>> state) : base(state) { }
}

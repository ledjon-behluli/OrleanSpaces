using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IGuidStore : ITupleStore<GuidTuple>, IGrainWithStringKey { }

internal sealed class GuidStore : BaseStore<GuidTuple>, IGuidStore
{
    public GuidStore(
        [PersistentState(Constants.RealmKey_Guid, Constants.StorageName)]
        IPersistentState<List<GuidTuple>> state) : base(state) { }
}

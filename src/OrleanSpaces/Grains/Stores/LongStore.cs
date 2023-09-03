using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface ILongStore : ITupleStore<LongTuple>, IGrainWithStringKey { }

internal sealed class LongStore : BaseStore<LongTuple>, ILongStore
{
    public LongStore(
        [PersistentState(Constants.RealmKey_Long, Constants.StorageName)]
        IPersistentState<List<LongTuple>> state) : base(Constants.RealmKey_Long, state) { }
}

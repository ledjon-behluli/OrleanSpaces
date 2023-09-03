using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IULongStore : ITupleStore<ULongTuple>, IGrainWithStringKey { }

internal sealed class ULongStore : BaseStore<ULongTuple>, IULongStore
{
    public ULongStore(
        [PersistentState(Constants.RealmKey_ULong, Constants.StorageName)]
        IPersistentState<List<ULongTuple>> state) : base(Constants.RealmKey_ULong, state) { }
}

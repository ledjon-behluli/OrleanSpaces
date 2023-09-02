using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IBoolStore : ITupleStore<BoolTuple>, IGrainWithStringKey { }

internal sealed class BoolStore : BaseStore<BoolTuple>, IBoolStore
{
    public BoolStore(
        [PersistentState(Constants.RealmKey_Bool, Constants.StorageName)]
        IPersistentState<List<BoolTuple>> space) : base(Constants.RealmKey_Bool, space) { }
}
using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IShortStore : ITupleStore<ShortTuple>, IGrainWithStringKey { }

internal sealed class ShortStore : BaseStore<ShortTuple>, IShortStore
{
    public ShortStore(
        [PersistentState(Constants.RealmKey_Short, Constants.StorageName)]
        IPersistentState<List<ShortTuple>> space) : base(Constants.RealmKey_Short, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IUShortStore : ITupleStore<UShortTuple>, IGrainWithStringKey { }

internal sealed class UShortStore : BaseStore<UShortTuple>, IUShortStore
{
    public UShortStore(
        [PersistentState(Constants.RealmKey_UShort, Constants.StorageName)]
        IPersistentState<List<UShortTuple>> state) : base(Constants.RealmKey_UShort, state) { }
}
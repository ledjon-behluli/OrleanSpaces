using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IUIntStore : ITupleStore<UIntTuple>, IGrainWithStringKey { }

internal sealed class UIntStore : BaseStore<UIntTuple>, IUIntStore
{
    public UIntStore(
        [PersistentState(Constants.RealmKey_UInt, Constants.StorageName)]
        IPersistentState<List<UIntTuple>> state) : base(state) { }
}

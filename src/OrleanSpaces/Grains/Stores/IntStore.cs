using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IIntStore : ITupleStore<IntTuple>, IGrainWithStringKey { }

internal sealed class IntStore : BaseStore<IntTuple>, IIntStore
{
    public IntStore(
        [PersistentState(Constants.RealmKey_Int, Constants.StorageName)]
        IPersistentState<List<IntTuple>> state) : base(state) { }
}
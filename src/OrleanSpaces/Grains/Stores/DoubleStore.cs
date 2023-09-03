using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IDoubleStore : ITupleStore<DoubleTuple>, IGrainWithStringKey { }

internal sealed class DoubleStore : BaseStore<DoubleTuple>, IDoubleStore
{
    public DoubleStore(
        [PersistentState(Constants.RealmKey_Double, Constants.StorageName)]
        IPersistentState<List<DoubleTuple>> state) : base(state) { }
}

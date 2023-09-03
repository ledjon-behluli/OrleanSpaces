using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IFloatStore : ITupleStore<FloatTuple>, IGrainWithStringKey { }

internal sealed class FloatStore : BaseStore<FloatTuple>, IFloatStore
{
    public FloatStore(
        [PersistentState(Constants.RealmKey_Float, Constants.StorageName)]
        IPersistentState<List<FloatTuple>> state) : base(Constants.RealmKey_Float, state) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IDecimalStore : ITupleStore<DecimalTuple>, IGrainWithStringKey { }

internal sealed class DecimalStore : BaseStore<DecimalTuple>, IDecimalStore
{
    public DecimalStore(
        [PersistentState(Constants.RealmKey_Decimal, Constants.StorageName)]
        IPersistentState<List<DecimalTuple>> space) : base(Constants.RealmKey_Decimal, space) { }
}

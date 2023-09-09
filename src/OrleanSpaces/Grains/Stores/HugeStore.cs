using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IHugeStore : ITupleStore<HugeTuple>, IGrainWithStringKey { }
internal sealed class HugeStore : BaseStore<HugeTuple>, IHugeStore
{
    public HugeStore(
        [PersistentState(Constants.RealmKey_Huge, Constants.StorageName)]
        IPersistentState<List<HugeTuple>> state) : base(state) { }
}

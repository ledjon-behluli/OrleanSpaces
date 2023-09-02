using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IUHugeStore : ITupleStore<UHugeTuple>, IGrainWithStringKey { }

internal sealed class UHugeStore : BaseStore<UHugeTuple>, IUHugeStore
{
    public UHugeStore(
        [PersistentState(Constants.RealmKey_UHuge, Constants.StorageName)]
        IPersistentState<List<UHugeTuple>> space) : base(Constants.RealmKey_UHuge, space) { }
}

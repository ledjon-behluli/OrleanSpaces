using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IByteStore : ITupleStore<ByteTuple>, IGrainWithStringKey { }

internal sealed class ByteStore : BaseStore<ByteTuple>, IByteStore
{
    public ByteStore(
        [PersistentState(Constants.RealmKey_Byte, Constants.StorageName)]
        IPersistentState<List<ByteTuple>> state) : base(state) { }
}
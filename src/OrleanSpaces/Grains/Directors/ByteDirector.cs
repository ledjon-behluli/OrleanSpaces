using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IByteDirector : IStoreDirector<ByteTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteDirector : BaseDirector<ByteTuple, IByteStore>, IByteDirector
{
    public ByteDirector(
        [PersistentState(Constants.RealmKey_Byte, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_Byte, storeFullKeys) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IByteDirector : IStoreDirector<ByteTuple>, IGrainWithStringKey
{
    const string Key = "ByteDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteDirector : BaseDirector<ByteTuple, IByteStore>, IByteDirector
{
    public ByteDirector(
        [PersistentState(IByteDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IByteStore.Key, storeIds) { }
}
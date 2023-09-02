using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IByteDirector : IStoreDirector<ByteTuple>, IGrainWithStringKey
{
    const string Key = "ByteDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteDirector : BaseDirector<ByteTuple, IByteGrain>, IByteDirector
{
    public ByteDirector(
        [PersistentState(IByteDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IByteGrain.Key, storeIds) { }
}
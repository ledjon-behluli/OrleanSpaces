using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IByteInterceptor : IStoreInterceptor<ByteTuple>, IGrainWithStringKey
{
    const string Key = "ByteInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteInterceptor : BaseInterceptor<ByteTuple, IByteGrain>, IByteInterceptor
{
    public ByteInterceptor(
        [PersistentState(IByteInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IByteGrain.Key, storeIds) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ISByteInterceptor : IStoreInterceptor<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class SByteInterceptor : BaseInterceptor<SByteTuple, ISByteGrain>, ISByteInterceptor
{
    public SByteInterceptor(
        [PersistentState(ISByteInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISByteGrain.Key, storeIds) { }
}

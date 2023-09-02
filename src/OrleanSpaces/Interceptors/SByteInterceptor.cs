using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ISByteInterceptor : IStoreInterceptor<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteInterceptor : BaseInterceptor<SByteTuple, ISByteGrain>, ISByteInterceptor
{
    public SByteInterceptor(
        [PersistentState(ISByteInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(ISByteGrain.Key, storeIds) { }
}

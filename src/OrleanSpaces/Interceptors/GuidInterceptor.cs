using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IGuidInterceptor : IStoreInterceptor<GuidTuple>, IGrainWithStringKey
{
    const string Key = "GuidInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidInterceptor : BaseInterceptor<GuidTuple, IGuidGrain>, IGuidInterceptor
{
    public GuidInterceptor(
        [PersistentState(IGuidInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IGuidGrain.Key, storeIds) { }
}
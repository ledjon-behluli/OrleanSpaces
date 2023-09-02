using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IHugeInterceptor : IStoreInterceptor<HugeTuple>, IGrainWithStringKey
{
    const string Key = "HugeInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeInterceptor : BaseInterceptor<HugeTuple, IHugeGrain>, IHugeInterceptor
{
    public HugeInterceptor(
        [PersistentState(IHugeInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IHugeGrain.Key, storeIds) { }
}

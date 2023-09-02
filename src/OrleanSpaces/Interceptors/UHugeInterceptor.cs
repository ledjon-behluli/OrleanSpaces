using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUHugeInterceptor : IStoreInterceptor<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeInterceptor : BaseInterceptor<UHugeTuple, IUHugeGrain>, IUHugeInterceptor
{
    public UHugeInterceptor(
        [PersistentState(IUHugeInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IUHugeGrain.Key, storeIds) { }
}

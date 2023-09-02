using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUHugeInterceptor : IStoreInterceptor<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class UHugeInterceptor : BaseInterceptor<UHugeTuple, IUHugeGrain>, IUHugeInterceptor
{
    public UHugeInterceptor(
        [PersistentState(IUHugeInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUHugeGrain.Key, storeIds) { }
}

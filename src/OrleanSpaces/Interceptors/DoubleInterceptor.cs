using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IDoubleInterceptor : IStoreInterceptor<DoubleTuple>, IGrainWithStringKey
{
    const string Key = "DoubleInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DoubleInterceptor : BaseInterceptor<DoubleTuple, IDoubleGrain>, IDoubleInterceptor
{
    public DoubleInterceptor(
        [PersistentState(IDoubleInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDoubleGrain.Key, storeIds) { }
}
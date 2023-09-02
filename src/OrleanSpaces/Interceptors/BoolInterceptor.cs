using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IBoolInterceptor : IStoreInterceptor<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class BoolInterceptor : BaseInterceptor<BoolTuple, IBoolGrain>, IBoolInterceptor
{
    public BoolInterceptor(
        [PersistentState(IBoolInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IBoolGrain.Key, storeIds) { }
}
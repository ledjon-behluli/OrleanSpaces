using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IBoolInterceptor : IStoreInterceptor<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolInterceptor : BaseInterceptor<BoolTuple, IBoolGrain>, IBoolInterceptor
{
    public BoolInterceptor(
        [PersistentState(IBoolInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IBoolGrain.Key, storeIds) { }
}
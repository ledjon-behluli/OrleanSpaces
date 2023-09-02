using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IFloatInterceptor : IStoreInterceptor<FloatTuple>, IGrainWithStringKey
{
    const string Key = "FloatInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatInterceptor : BaseInterceptor<FloatTuple, IFloatGrain>, IFloatInterceptor
{
    public FloatInterceptor(
        [PersistentState(IFloatInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IFloatGrain.Key, storeIds) { }
}
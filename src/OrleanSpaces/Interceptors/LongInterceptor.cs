using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ILongInterceptor : IStoreInterceptor<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongInterceptor : BaseInterceptor<LongTuple, ILongGrain>, ILongInterceptor
{
    public LongInterceptor(
        [PersistentState(ILongInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(ILongGrain.Key, storeIds) { }
}

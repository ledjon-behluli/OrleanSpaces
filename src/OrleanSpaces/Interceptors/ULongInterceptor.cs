using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IULongInterceptor : IStoreInterceptor<ULongTuple>, IGrainWithStringKey
{
    const string Key = "ULongInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongInterceptor : BaseInterceptor<ULongTuple, IULongGrain>, IULongInterceptor
{
    public ULongInterceptor(
        [PersistentState(IULongInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IULongGrain.Key, storeIds) { }
}

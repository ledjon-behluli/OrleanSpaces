using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IULongInterceptor : IStoreInterceptor<ULongTuple>, IGrainWithStringKey
{
    const string Key = "ULongInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class ULongInterceptor : BaseInterceptor<ULongTuple, IULongGrain>, IULongInterceptor
{
    public ULongInterceptor(
        [PersistentState(IULongInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IULongGrain.Key, storeIds) { }
}

using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ILongInterceptor : IStoreInterceptor<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class LongInterceptor : BaseInterceptor<LongTuple, ILongGrain>, ILongInterceptor
{
    public LongInterceptor(
        [PersistentState(ILongInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ILongGrain.Key, storeIds) { }
}

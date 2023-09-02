using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IIntInterceptor : IStoreInterceptor<IntTuple>, IGrainWithStringKey
{
    const string Key = "IntInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class IntInterceptor : BaseInterceptor<IntTuple, IIntGrain>, IIntInterceptor
{
    public IntInterceptor(
        [PersistentState(IIntInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IIntGrain.Key, storeIds) { }
}

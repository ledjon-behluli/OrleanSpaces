using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUIntInterceptor : IStoreInterceptor<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class UIntInterceptor : BaseInterceptor<UIntTuple, IUIntGrain>, IUIntInterceptor
{
    public UIntInterceptor(
        [PersistentState(IUIntInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUIntGrain.Key, storeIds) { }
}

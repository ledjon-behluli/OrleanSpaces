using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUShortInterceptor : IStoreInterceptor<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class UShortInterceptor : BaseInterceptor<UShortTuple, IUShortGrain>, IUShortInterceptor
{
    public UShortInterceptor(
        [PersistentState(IUShortInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUShortGrain.Key, storeIds) { }
}
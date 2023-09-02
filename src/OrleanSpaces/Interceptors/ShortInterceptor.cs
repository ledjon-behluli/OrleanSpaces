using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IShortInterceptor : IStoreInterceptor<ShortTuple>, IGrainWithStringKey
{
    const string Key = "ShortInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class ShortInterceptor : BaseInterceptor<ShortTuple, IShortGrain>, IShortInterceptor
{
    public ShortInterceptor(
        [PersistentState(IShortInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IShortGrain.Key, storeIds) { }
}

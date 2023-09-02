using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IDecimalInterceptor : IStoreInterceptor<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DecimalInterceptor : BaseInterceptor<DecimalTuple, IDecimalGrain>, IDecimalInterceptor
{
    public DecimalInterceptor(
        [PersistentState(IDecimalInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDecimalGrain.Key, storeIds) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IDecimalInterceptor : IStoreInterceptor<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalInterceptor : BaseInterceptor<DecimalTuple, IDecimalGrain>, IDecimalInterceptor
{
    public DecimalInterceptor(
        [PersistentState(IDecimalInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDecimalGrain.Key, storeIds) { }
}
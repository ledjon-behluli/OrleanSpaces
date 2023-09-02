using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUShortInterceptor : IStoreInterceptor<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortInterceptor : BaseInterceptor<UShortTuple, IUShortGrain>, IUShortInterceptor
{
    public UShortInterceptor(
        [PersistentState(IUShortInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IUShortGrain.Key, storeIds) { }
}
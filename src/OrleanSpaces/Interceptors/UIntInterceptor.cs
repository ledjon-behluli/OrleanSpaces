using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IUIntInterceptor : IStoreInterceptor<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntInterceptor : BaseInterceptor<UIntTuple, IUIntGrain>, IUIntInterceptor
{
    public UIntInterceptor(
        [PersistentState(IUIntInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(IUIntGrain.Key, storeIds) { }
}

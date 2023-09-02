using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ICharInterceptor : IStoreInterceptor<CharTuple>, IGrainWithStringKey
{
    const string Key = "CharInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharInterceptor : BaseInterceptor<CharTuple, ICharGrain>, ICharInterceptor
{
    public CharInterceptor(
        [PersistentState(ICharInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ICharGrain.Key, storeIds) { }
}
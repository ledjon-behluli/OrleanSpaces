using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Interceptors;

internal interface ISpaceInterceptor : IStoreInterceptor<SpaceTuple>, IGrainWithStringKey
{
    const string Key = "SpaceInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class SpaceInterceptor : BaseInterceptor<SpaceTuple, ISpaceGrain>, ISpaceInterceptor
{
    public SpaceInterceptor(
        [PersistentState(ISpaceInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ISpaceGrain.Key, storeIds) { }
}

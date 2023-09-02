using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IUHugeDirector : IStoreDirector<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeDirector : BaseDirector<UHugeTuple, IUHugeGrain>, IUHugeDirector
{
    public UHugeDirector(
        [PersistentState(IUHugeDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUHugeGrain.Key, storeIds) { }
}

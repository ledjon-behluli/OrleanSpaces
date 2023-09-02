using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IHugeDirector : IStoreDirector<HugeTuple>, IGrainWithStringKey
{
    const string Key = "HugeDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeDirector : BaseDirector<HugeTuple, IHugeGrain>, IHugeDirector
{
    public HugeDirector(
        [PersistentState(IHugeDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IHugeGrain.Key, storeIds) { }
}

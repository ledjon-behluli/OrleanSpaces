using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IHugeDirector : IStoreDirector<HugeTuple>, IGrainWithStringKey
{
    const string Key = "HugeDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeDirector : BaseDirector<HugeTuple, IHugeStore>, IHugeDirector
{
    public HugeDirector(
        [PersistentState(IHugeDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IHugeStore.Key, storeIds) { }
}

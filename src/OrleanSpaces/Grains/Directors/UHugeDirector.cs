using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUHugeDirector : IStoreDirector<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeDirector : BaseDirector<UHugeTuple, IUHugeStore>, IUHugeDirector
{
    public UHugeDirector(
        [PersistentState(IUHugeDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUHugeStore.Key, storeIds) { }
}

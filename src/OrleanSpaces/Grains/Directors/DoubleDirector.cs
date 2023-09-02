using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDoubleDirector : IStoreDirector<DoubleTuple>, IGrainWithStringKey
{
    const string Key = "DoubleDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleDirector : BaseDirector<DoubleTuple, IDoubleStore>, IDoubleDirector
{
    public DoubleDirector(
        [PersistentState(IDoubleDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDoubleStore.Key, storeIds) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IDoubleDirector : IStoreDirector<DoubleTuple>, IGrainWithStringKey
{
    const string Key = "DoubleDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleDirector : BaseDirector<DoubleTuple, IDoubleGrain>, IDoubleDirector
{
    public DoubleDirector(
        [PersistentState(IDoubleDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDoubleGrain.Key, storeIds) { }
}
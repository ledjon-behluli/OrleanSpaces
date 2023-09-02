using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IIntDirector : IStoreDirector<IntTuple>, IGrainWithStringKey
{
    const string Key = "IntDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntDirector : BaseDirector<IntTuple, IIntStore>, IIntDirector
{
    public IntDirector(
        [PersistentState(IIntDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IIntStore.Key, storeIds) { }
}

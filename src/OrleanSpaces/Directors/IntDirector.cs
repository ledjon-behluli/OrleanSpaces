using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IIntDirector : IStoreDirector<IntTuple>, IGrainWithStringKey
{
    const string Key = "IntDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntDirector : BaseDirector<IntTuple, IIntGrain>, IIntDirector
{
    public IntDirector(
        [PersistentState(IIntDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IIntGrain.Key, storeIds) { }
}

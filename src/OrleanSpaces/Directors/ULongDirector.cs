using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IULongDirector : IStoreDirector<ULongTuple>, IGrainWithStringKey
{
    const string Key = "ULongDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongDirector : BaseDirector<ULongTuple, IULongGrain>, IULongDirector
{
    public ULongDirector(
        [PersistentState(IULongDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IULongGrain.Key, storeIds) { }
}

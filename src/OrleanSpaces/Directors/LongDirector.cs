using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface ILongDirector : IStoreDirector<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongDirector : BaseDirector<LongTuple, ILongGrain>, ILongDirector
{
    public LongDirector(
        [PersistentState(ILongDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ILongGrain.Key, storeIds) { }
}

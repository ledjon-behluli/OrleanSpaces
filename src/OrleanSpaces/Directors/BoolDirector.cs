using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IBoolDirector : IStoreDirector<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolDirector : BaseDirector<BoolTuple, IBoolGrain>, IBoolDirector
{
    public BoolDirector(
        [PersistentState(IBoolDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IBoolGrain.Key, storeIds) { }
}
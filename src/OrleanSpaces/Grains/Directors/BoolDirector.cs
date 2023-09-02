using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IBoolDirector : IStoreDirector<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolDirector : BaseDirector<BoolTuple, IBoolStore>, IBoolDirector
{
    public BoolDirector(
        [PersistentState(IBoolDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IBoolStore.Key, storeIds) { }
}
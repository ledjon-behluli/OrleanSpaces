using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IShortDirector : IStoreDirector<ShortTuple>, IGrainWithStringKey
{
    const string Key = "ShortDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortDirector : BaseDirector<ShortTuple, IShortGrain>, IShortDirector
{
    public ShortDirector(
        [PersistentState(IShortDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IShortGrain.Key, storeIds) { }
}

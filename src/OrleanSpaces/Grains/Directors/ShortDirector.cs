using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IShortDirector : IStoreDirector<ShortTuple>, IGrainWithStringKey
{
    const string Key = "ShortDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortDirector : BaseDirector<ShortTuple, IShortStore>, IShortDirector
{
    public ShortDirector(
        [PersistentState(IShortDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IShortStore.Key, storeIds) { }
}

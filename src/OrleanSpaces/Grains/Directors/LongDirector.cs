using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ILongDirector : IStoreDirector<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongDirector : BaseDirector<LongTuple, ILongStore>, ILongDirector
{
    public LongDirector(
        [PersistentState(ILongDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ILongStore.Key, storeIds) { }
}

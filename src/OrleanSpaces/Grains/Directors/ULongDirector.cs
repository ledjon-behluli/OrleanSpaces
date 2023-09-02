using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IULongDirector : IStoreDirector<ULongTuple>, IGrainWithStringKey
{
    const string Key = "ULongDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongDirector : BaseDirector<ULongTuple, IULongStore>, IULongDirector
{
    public ULongDirector(
        [PersistentState(IULongDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IULongStore.Key, storeIds) { }
}

using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUShortDirector : IStoreDirector<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortDirector : BaseDirector<UShortTuple, IUShortStore>, IUShortDirector
{
    public UShortDirector(
        [PersistentState(IUShortDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUShortStore.Key, storeIds) { }
}
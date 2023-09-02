using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IUShortDirector : IStoreDirector<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortDirector : BaseDirector<UShortTuple, IUShortGrain>, IUShortDirector
{
    public UShortDirector(
        [PersistentState(IUShortDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUShortGrain.Key, storeIds) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUIntDirector : IStoreDirector<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntDirector : BaseDirector<UIntTuple, IUIntStore>, IUIntDirector
{
    public UIntDirector(
        [PersistentState(IUIntDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUIntStore.Key, storeIds) { }
}

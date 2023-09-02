using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IUIntDirector : IStoreDirector<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntDirector : BaseDirector<UIntTuple, IUIntGrain>, IUIntDirector
{
    public UIntDirector(
        [PersistentState(IUIntDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IUIntGrain.Key, storeIds) { }
}

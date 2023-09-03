using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUIntDirector : IStoreDirector<UIntTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntDirector : BaseDirector<UIntTuple, IUIntStore>, IUIntDirector
{
    public UIntDirector(
        [PersistentState(Constants.RealmKey_UInt, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_UInt, storeFullKeys) { }
}

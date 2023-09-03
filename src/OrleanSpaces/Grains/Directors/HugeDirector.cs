using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IHugeDirector : IStoreDirector<HugeTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeDirector : BaseDirector<HugeTuple, IHugeStore>, IHugeDirector
{
    public HugeDirector(
        [PersistentState(Constants.RealmKey_Huge, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_Huge, storeFullKeys) { }
}

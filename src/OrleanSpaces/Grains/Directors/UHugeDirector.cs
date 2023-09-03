using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUHugeDirector : IStoreDirector<UHugeTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeDirector : BaseDirector<UHugeTuple, IUHugeStore>, IUHugeDirector
{
    public UHugeDirector(
        [PersistentState(Constants.RealmKey_UHuge, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_UHuge, storeKeys) { }
}

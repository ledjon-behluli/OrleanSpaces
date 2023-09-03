using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ILongDirector : IStoreDirector<LongTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongDirector : BaseDirector<LongTuple, ILongStore>, ILongDirector
{
    public LongDirector(
        [PersistentState(Constants.RealmKey_Long, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_Long, storeKeys) { }
}

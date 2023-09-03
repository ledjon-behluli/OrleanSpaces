using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IBoolDirector : IStoreDirector<BoolTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolDirector : BaseDirector<BoolTuple, IBoolStore>, IBoolDirector
{
    public BoolDirector(
        [PersistentState(Constants.RealmKey_Bool, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_Bool, storeKeys) { }
}
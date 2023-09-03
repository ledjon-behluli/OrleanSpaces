using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IIntDirector : IStoreDirector<IntTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntDirector : BaseDirector<IntTuple, IIntStore>, IIntDirector
{
    public IntDirector(
        [PersistentState(Constants.RealmKey_Int, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_Int, storeFullKeys) { }
}

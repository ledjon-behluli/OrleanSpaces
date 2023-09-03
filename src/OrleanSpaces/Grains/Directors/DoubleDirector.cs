using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDoubleDirector : IStoreDirector<DoubleTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleDirector : BaseDirector<DoubleTuple, IDoubleStore>, IDoubleDirector
{
    public DoubleDirector(
        [PersistentState(Constants.RealmKey_Double, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_Double, storeKeys) { }
}
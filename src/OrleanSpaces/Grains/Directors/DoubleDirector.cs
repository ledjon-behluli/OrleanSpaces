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
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Double, state) {}
}
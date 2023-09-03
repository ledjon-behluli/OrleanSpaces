using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IFloatDirector : IStoreDirector<FloatTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatDirector : BaseDirector<FloatTuple, IFloatStore>, IFloatDirector
{
    public FloatDirector(
        [PersistentState(Constants.RealmKey_Float, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Float, state) {}
}
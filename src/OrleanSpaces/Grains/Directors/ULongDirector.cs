using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IULongDirector : IStoreDirector<ULongTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongDirector : BaseDirector<ULongTuple, IULongStore>, IULongDirector
{
    public ULongDirector(
        [PersistentState(Constants.RealmKey_ULong, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_ULong, state) {}
}

using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IGuidDirector : IStoreDirector<GuidTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidDirector : BaseDirector<GuidTuple, IGuidStore>, IGuidDirector
{
    public GuidDirector(
        [PersistentState(Constants.RealmKey_Guid, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Guid, state) {}
}
using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUShortDirector : IStoreDirector<UShortTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortDirector : BaseDirector<UShortTuple, IUShortStore>, IUShortDirector
{
    public UShortDirector(
        [PersistentState(Constants.RealmKey_UShort, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_UShort, state) {}
}
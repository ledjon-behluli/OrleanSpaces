using Orleans.Runtime;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Grains.Stores;

internal interface ISpaceStore : ITupleStore<SpaceTuple>, IGrainWithStringKey { }

internal sealed class SpaceStore : BaseStore<SpaceTuple>, ISpaceStore
{
    public SpaceStore(
        [PersistentState(Constants.RealmKey_Space, Constants.StorageName)]
        IPersistentState<List<SpaceTuple>> state) : base(Constants.RealmKey_Space, state) { }
}
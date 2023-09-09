using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Grains.Directors;

internal interface ISpaceDirector : IStoreDirector<SpaceTuple>, IGrainWithStringKey { }

internal sealed class SpaceDirector : BaseDirector<SpaceTuple, ISpaceStore>, ISpaceDirector
{
    public SpaceDirector(
        [PersistentState(Constants.RealmKey_Space, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Space, state) {}
}

using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ILongDirector : IStoreDirector<LongTuple>, IGrainWithStringKey { }

internal sealed class LongDirector : BaseDirector<LongTuple, ILongStore>, ILongDirector
{
    public LongDirector(
        [PersistentState(Constants.RealmKey_Long, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Long, state) {}
}

using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IShortDirector : IStoreDirector<ShortTuple>, IGrainWithStringKey { }

internal sealed class ShortDirector : BaseDirector<ShortTuple, IShortStore>, IShortDirector
{
    public ShortDirector(
        [PersistentState(Constants.RealmKey_Short, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Short, state) {}
}

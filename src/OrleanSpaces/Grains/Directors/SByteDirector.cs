using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ISByteDirector : IStoreDirector<SByteTuple>, IGrainWithStringKey { }

internal sealed class SByteDirector : BaseDirector<SByteTuple, ISByteStore>, ISByteDirector
{
    public SByteDirector(
        [PersistentState(Constants.RealmKey_SByte, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_SByte, state) {}
}

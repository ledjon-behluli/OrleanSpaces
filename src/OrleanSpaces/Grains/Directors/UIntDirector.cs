using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IUIntDirector : IStoreDirector<UIntTuple>, IGrainWithStringKey { }

internal sealed class UIntDirector : BaseDirector<UIntTuple, IUIntStore>, IUIntDirector
{
    public UIntDirector(
        [PersistentState(Constants.RealmKey_UInt, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_UInt, state) {}
}

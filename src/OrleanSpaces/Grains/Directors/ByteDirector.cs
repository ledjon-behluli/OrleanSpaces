using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IByteDirector : IStoreDirector<ByteTuple>, IGrainWithStringKey { }

internal sealed class ByteDirector : BaseDirector<ByteTuple, IByteStore>, IByteDirector
{
    public ByteDirector(
        [PersistentState(Constants.RealmKey_Byte, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Byte, state) {}
}
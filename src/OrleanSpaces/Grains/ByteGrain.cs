using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IByteGrain : ITupleStore<ByteTuple>, IGrainWithStringKey
{
    const string Key = "ByteStore";
}

internal sealed class ByteGrain : BaseGrain<ByteTuple>, IByteGrain
{
    public ByteGrain(
        [PersistentState(IByteGrain.Key, Constants.StorageName)]
        IPersistentState<List<ByteTuple>> space) : base(IByteGrain.Key, space) { }
}
using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface ISByteGrain : ITupleStore<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteStore";
}

internal sealed class SByteGrain : BaseGrain<SByteTuple>, ISByteGrain
{
    public SByteGrain(
        [PersistentState(ISByteGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<SByteTuple>> space) : base(ISByteGrain.Key, space) { }
}

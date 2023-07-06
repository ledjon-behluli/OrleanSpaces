using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface ISByteGrain : ITupleStore<SByteTuple>, IGrainWithStringKey
{
    const string Key = "SByteStore";
}

internal sealed class SByteGrain : Grain<SByteTuple>, ISByteGrain
{
    public SByteGrain(
        [PersistentState(ISByteGrain.Key, Constants.StorageName)]
        IPersistentState<HashSet<SByteTuple>> space) : base(ISByteGrain.Key, space) { }
}

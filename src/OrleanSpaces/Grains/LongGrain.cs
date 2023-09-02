using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface ILongGrain : ITupleStore<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongStore";
}

internal sealed class LongGrain : BaseGrain<LongTuple>, ILongGrain
{
    public LongGrain(
        [PersistentState(ILongGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<LongTuple>> space) : base(ILongGrain.Key, space) { }
}

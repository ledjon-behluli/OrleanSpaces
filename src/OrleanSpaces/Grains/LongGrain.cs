using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface ILongGrain : ITupleStore<LongTuple>, IGrainWithStringKey
{
    const string Key = "LongStore";
}

internal sealed class LongGrain : Grain<LongTuple>, ILongGrain
{
    public LongGrain(
        [PersistentState(ILongGrain.Key, Constants.StorageName)]
        IPersistentState<List<LongTuple>> space) : base(ILongGrain.Key, space) { }
}

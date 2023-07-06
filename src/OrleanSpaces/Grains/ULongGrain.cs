using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IULongGrain : ITupleStore<ULongTuple>, IGrainWithStringKey
{
    const string Key = "ULongStore";
}

internal sealed class ULongGrain : Grain<ULongTuple>, IULongGrain
{
    public ULongGrain(
        [PersistentState(IULongGrain.Key, Constants.StorageName)]
        IPersistentState<HashSet<ULongTuple>> space) : base(IULongGrain.Key, space) { }
}

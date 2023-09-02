using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IUShortGrain : ITupleStore<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortStore";
}


internal sealed class UShortGrain : BaseGrain<UShortTuple>, IUShortGrain
{
    public UShortGrain(
        [PersistentState(IUShortGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<UShortTuple>> space) : base(IUShortGrain.Key, space) { }
}
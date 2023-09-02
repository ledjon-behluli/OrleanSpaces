using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : ITupleStore<IntTuple>, IGrainWithStringKey 
{
    const string Key = "IntStore";
}

internal sealed class IntGrain : BaseGrain<IntTuple>, IIntGrain
{
    public IntGrain(
        [PersistentState(IIntGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<IntTuple>> space) : base(IIntGrain.Key, space) { }
}
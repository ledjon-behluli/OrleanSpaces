using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IIntGrain : ITupleStore<IntTuple>, IGrainWithStringKey 
{
    const string Key = "IntStore";
}

internal sealed class IntGrain : Grain<IntTuple>, IIntGrain
{
    public IntGrain(
        [PersistentState(IIntGrain.Key, Constants.StorageName)]
        IPersistentState<List<IntTuple>> space) : base(IIntGrain.Key, space) { }
}
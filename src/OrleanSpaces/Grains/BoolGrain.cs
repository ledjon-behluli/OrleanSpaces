using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IBoolGrain : ITupleStore<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolStore";
}

internal sealed class BoolGrain : BaseGrain<BoolTuple>, IBoolGrain
{
    public BoolGrain(
        [PersistentState(IBoolGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<BoolTuple>> space) : base(IBoolGrain.Key, space) { }
}
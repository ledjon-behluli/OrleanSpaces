using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IBoolGrain : ITupleStore<BoolTuple>, IGrainWithStringKey
{
    const string Key = "BoolStore";
}

internal sealed class BoolGrain : Grain<BoolTuple>, IBoolGrain
{
    public BoolGrain(
        [PersistentState(IBoolGrain.Key, Constants.StorageName)]
        IPersistentState<List<BoolTuple>> space) : base(IBoolGrain.Key, space) { }
}
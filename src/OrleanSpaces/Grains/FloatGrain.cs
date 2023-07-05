using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IFloatGrain : ITupleStore<FloatTuple>, IGrainWithStringKey
{
    const string Key = "FloatStore";
}

internal sealed class FloatGrain : Grain<FloatTuple>, IFloatGrain
{
    public FloatGrain(
        [PersistentState(IFloatGrain.Key, Constants.StorageName)]
        IPersistentState<List<FloatTuple>> space) : base(IFloatGrain.Key, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IDecimalGrain : ITupleStore<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalStore";
}

internal sealed class DecimalGrain : Grain<DecimalTuple>, IDecimalGrain
{
    public DecimalGrain(
        [PersistentState(IDecimalGrain.Key, Constants.StorageName)]
        IPersistentState<List<DecimalTuple>> space) : base(IDecimalGrain.Key, space) { }
}

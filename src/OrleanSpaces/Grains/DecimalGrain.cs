using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IDecimalGrain : ITupleStore<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalStore";
}

internal sealed class DecimalGrain : BaseGrain<DecimalTuple>, IDecimalGrain
{
    public DecimalGrain(
        [PersistentState(IDecimalGrain.Key, Constants.StorageName)]
        IPersistentState<List<DecimalTuple>> space) : base(IDecimalGrain.Key, space) { }
}

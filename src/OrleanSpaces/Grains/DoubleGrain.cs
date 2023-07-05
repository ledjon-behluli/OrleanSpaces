using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IDoubleGrain : ITupleStore<DoubleTuple>, IGrainWithStringKey
{
    const string Key = "DoubleStore";
}

internal sealed class DoubleGrain : Grain<DoubleTuple>, IDoubleGrain
{
    public DoubleGrain(
        [PersistentState(IDoubleGrain.Key, Constants.StorageName)]
        IPersistentState<List<DoubleTuple>> space) : base(IDoubleGrain.Key, space) { }
}

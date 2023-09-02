using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IDoubleGrain : ITupleStore<DoubleTuple>, IGrainWithStringKey
{
    const string Key = "DoubleStore";
}

internal sealed class DoubleGrain : BaseGrain<DoubleTuple>, IDoubleGrain
{
    public DoubleGrain(
        [PersistentState(IDoubleGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<DoubleTuple>> space) : base(IDoubleGrain.Key, space) { }
}

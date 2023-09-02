using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IHugeGrain : ITupleStore<HugeTuple>, IGrainWithStringKey
{
    const string Key = "HugeStore";
}

internal sealed class HugeGrain : BaseGrain<HugeTuple>, IHugeGrain
{
    public HugeGrain(
        [PersistentState(IHugeGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<HugeTuple>> space) : base(IHugeGrain.Key, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IHugeGrain : ITupleStore<HugeTuple>, IGrainWithStringKey
{
    const string Key = "HugeStore";
}

internal sealed class HugeGrain : Grain<HugeTuple>, IHugeGrain
{
    public HugeGrain(
        [PersistentState(IHugeGrain.Key, Constants.StorageName)]
        IPersistentState<List<HugeTuple>> space) : base(IHugeGrain.Key, space) { }
}

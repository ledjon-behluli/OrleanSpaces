using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IUHugeGrain : ITupleStore<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeStore";
}

internal sealed class UHugeGrain : BaseGrain<UHugeTuple>, IUHugeGrain
{
    public UHugeGrain(
        [PersistentState(IUHugeGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<UHugeTuple>> space) : base(IUHugeGrain.Key, space) { }
}

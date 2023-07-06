using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IUHugeGrain : ITupleStore<UHugeTuple>, IGrainWithStringKey
{
    const string Key = "UHugeStore";
}

internal sealed class UHugeGrain : Grain<UHugeTuple>, IUHugeGrain
{
    public UHugeGrain(
        [PersistentState(IUHugeGrain.Key, Constants.StorageName)]
        IPersistentState<HashSet<UHugeTuple>> space) : base(IUHugeGrain.Key, space) { }
}

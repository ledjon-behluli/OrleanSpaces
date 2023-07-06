using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IUIntGrain : ITupleStore<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntStore";
}

internal sealed class UIntGrain : Grain<UIntTuple>, IUIntGrain
{
    public UIntGrain(
        [PersistentState(IUIntGrain.Key, Constants.StorageName)]
        IPersistentState<HashSet<UIntTuple>> space) : base(IUIntGrain.Key, space) { }
}

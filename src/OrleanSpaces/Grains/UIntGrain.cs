using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IUIntGrain : ITupleStore<UIntTuple>, IGrainWithStringKey
{
    const string Key = "UIntStore";
}

internal sealed class UIntGrain : BaseGrain<UIntTuple>, IUIntGrain
{
    public UIntGrain(
        [PersistentState(IUIntGrain.Key, Constants.StorageName)]
        IPersistentState<List<UIntTuple>> space) : base(IUIntGrain.Key, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface ICharGrain : ITupleStore<CharTuple>, IGrainWithStringKey
{
    const string Key = "CharStore";
}

internal sealed class CharGrain : BaseGrain<CharTuple>, ICharGrain
{
    public CharGrain(
        [PersistentState(ICharGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<CharTuple>> space) : base(ICharGrain.Key, space) { }
}

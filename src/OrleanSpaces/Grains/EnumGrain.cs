using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IEnumGrain : ITupleStore<EnumTuple>, IGrainWithStringKey
{
    const string Key = "EnumStore";
}

internal sealed class EnumGrain : Grain<EnumTuple>, IEnumGrain
{
    public EnumGrain(
        [PersistentState(IEnumGrain.Key, Constants.StorageName)]
        IPersistentState<List<EnumTuple>> space) : base(IEnumGrain.Key, space) { }
}
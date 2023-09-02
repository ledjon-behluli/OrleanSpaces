using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IFloatDirector : IStoreDirector<FloatTuple>, IGrainWithStringKey
{
    const string Key = "FloatDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatDirector : BaseDirector<FloatTuple, IFloatGrain>, IFloatDirector
{
    public FloatDirector(
        [PersistentState(IFloatDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IFloatGrain.Key, storeIds) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IFloatDirector : IStoreDirector<FloatTuple>, IGrainWithStringKey
{
    const string Key = "FloatDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatDirector : BaseDirector<FloatTuple, IFloatStore>, IFloatDirector
{
    public FloatDirector(
        [PersistentState(IFloatDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IFloatStore.Key, storeIds) { }
}
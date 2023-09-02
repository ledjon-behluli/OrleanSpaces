using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ICharDirector : IStoreDirector<CharTuple>, IGrainWithStringKey
{
    const string Key = "CharDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharDirector : BaseDirector<CharTuple, ICharGrain>, ICharDirector
{
    public CharDirector(
        [PersistentState(ICharDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ICharGrain.Key, storeIds) { }
}
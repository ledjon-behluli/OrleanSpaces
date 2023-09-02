using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IDecimalDirector : IStoreDirector<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalDirector : BaseDirector<DecimalTuple, IDecimalGrain>, IDecimalDirector
{
    public DecimalDirector(
        [PersistentState(IDecimalDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDecimalGrain.Key, storeIds) { }
}
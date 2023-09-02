using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDecimalDirector : IStoreDirector<DecimalTuple>, IGrainWithStringKey
{
    const string Key = "DecimalDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalDirector : BaseDirector<DecimalTuple, IDecimalStore>, IDecimalDirector
{
    public DecimalDirector(
        [PersistentState(IDecimalDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDecimalStore.Key, storeIds) { }
}
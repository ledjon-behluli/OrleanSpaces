using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDecimalDirector : IStoreDirector<DecimalTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalDirector : BaseDirector<DecimalTuple, IDecimalStore>, IDecimalDirector
{
    public DecimalDirector(
        [PersistentState(Constants.RealmKey_Decimal, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_Decimal, storeKeys) { }
}
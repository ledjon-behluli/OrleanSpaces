using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDateTimeOffsetDirector : IStoreDirector<DateTimeOffsetTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetDirector : BaseDirector<DateTimeOffsetTuple, IDateTimeOffsetStore>, IDateTimeOffsetDirector
{
    public DateTimeOffsetDirector(
        [PersistentState(Constants.RealmKey_DateTimeOffset, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeFullKeys)
        : base(Constants.RealmKey_DateTimeOffset, storeFullKeys) { }
}
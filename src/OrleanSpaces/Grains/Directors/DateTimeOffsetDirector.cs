using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDateTimeOffsetDirector : IStoreDirector<DateTimeOffsetTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeOffsetDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetDirector : BaseDirector<DateTimeOffsetTuple, IDateTimeOffsetStore>, IDateTimeOffsetDirector
{
    public DateTimeOffsetDirector(
        [PersistentState(IDateTimeOffsetDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDateTimeOffsetStore.Key, storeIds) { }
}
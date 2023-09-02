using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IDateTimeOffsetDirector : IStoreDirector<DateTimeOffsetTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeOffsetDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetDirector : BaseDirector<DateTimeOffsetTuple, IDateTimeOffsetGrain>, IDateTimeOffsetDirector
{
    public DateTimeOffsetDirector(
        [PersistentState(IDateTimeOffsetDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDateTimeOffsetGrain.Key, storeIds) { }
}
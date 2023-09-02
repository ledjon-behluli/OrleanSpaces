using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface IDateTimeDirector : IStoreDirector<DateTimeTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeDirector : BaseDirector<DateTimeTuple, IDateTimeGrain>, IDateTimeDirector
{
    public DateTimeDirector(
        [PersistentState(IDateTimeDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDateTimeGrain.Key, storeIds) { }
}
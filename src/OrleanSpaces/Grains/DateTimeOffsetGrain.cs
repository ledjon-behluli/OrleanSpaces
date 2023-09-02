using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IDateTimeOffsetGrain : ITupleStore<DateTimeOffsetTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeOffsetStore";
}

internal sealed class DateTimeOffsetGrain : BaseGrain<DateTimeOffsetTuple>, IDateTimeOffsetGrain
{
    public DateTimeOffsetGrain(
        [PersistentState(IDateTimeOffsetGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<DateTimeOffsetTuple>> space) : base(IDateTimeOffsetGrain.Key, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IDateTimeOffsetGrain : ITupleStore<DateTimeOffsetTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeOffsetStore";
}

internal sealed class DateTimeOffsetGrain : Grain<DateTimeOffsetTuple>, IDateTimeOffsetGrain
{
    public DateTimeOffsetGrain(
        [PersistentState(IDateTimeOffsetGrain.Key, Constants.StorageName)]
        IPersistentState<List<DateTimeOffsetTuple>> space) : base(IDateTimeOffsetGrain.Key, space) { }
}

using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IDateTimeGrain : ITupleStore<DateTimeTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeStore";
}

internal sealed class DateTimeGrain : BaseGrain<DateTimeTuple>, IDateTimeGrain
{
    public DateTimeGrain(
        [PersistentState(IDateTimeGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<DateTimeTuple>> space) : base(IDateTimeGrain.Key, space) { }
}

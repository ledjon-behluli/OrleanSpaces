using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IDateTimeGrain : ITupleStore<DateTimeTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeStore";
}

internal sealed class DateTimeGrain : Grain<DateTimeTuple>, IDateTimeGrain
{
    public DateTimeGrain(
        [PersistentState(IDateTimeGrain.Key, Constants.StorageName)]
        IPersistentState<List<DateTimeTuple>> space) : base(IDateTimeGrain.Key, space) { }
}

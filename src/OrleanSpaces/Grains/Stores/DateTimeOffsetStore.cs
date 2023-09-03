using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IDateTimeOffsetStore : ITupleStore<DateTimeOffsetTuple>, IGrainWithStringKey { }

internal sealed class DateTimeOffsetStore : BaseStore<DateTimeOffsetTuple>, IDateTimeOffsetStore
{
    public DateTimeOffsetStore(
        [PersistentState(Constants.RealmKey_DateTimeOffset, Constants.StorageName)]
        IPersistentState<List<DateTimeOffsetTuple>> state) : base(state) { }
}

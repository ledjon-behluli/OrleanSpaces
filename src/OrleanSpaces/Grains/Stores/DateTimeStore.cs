using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface IDateTimeStore : ITupleStore<DateTimeTuple>, IGrainWithStringKey { }

internal sealed class DateTimeStore : BaseStore<DateTimeTuple>, IDateTimeStore
{
    public DateTimeStore(
        [PersistentState(Constants.RealmKey_DateTime, Constants.StorageName)]
        IPersistentState<List<DateTimeTuple>> state) : base(Constants.RealmKey_DateTime, state) { }
}

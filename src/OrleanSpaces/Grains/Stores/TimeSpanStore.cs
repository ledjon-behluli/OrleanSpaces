using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface ITimeSpanStore : ITupleStore<TimeSpanTuple>, IGrainWithStringKey { }

internal sealed class TimeSpanStore : BaseStore<TimeSpanTuple>, ITimeSpanStore
{
    public TimeSpanStore(
        [PersistentState(Constants.RealmKey_TimeSpan, Constants.StorageName)]
        IPersistentState<List<TimeSpanTuple>> space) : base(Constants.RealmKey_TimeSpan, space) { }
}

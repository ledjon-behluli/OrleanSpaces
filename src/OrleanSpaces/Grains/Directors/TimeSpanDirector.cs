using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ITimeSpanDirector : IStoreDirector<TimeSpanTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanDirector : BaseDirector<TimeSpanTuple, ITimeSpanStore>, ITimeSpanDirector
{
    public TimeSpanDirector(
        [PersistentState(Constants.RealmKey_TimeSpan, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_TimeSpan, storeKeys) { }
}
using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Directors;

internal interface ITimeSpanDirector : IStoreDirector<TimeSpanTuple>, IGrainWithStringKey
{
    const string Key = "TimeSpanDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanDirector : BaseDirector<TimeSpanTuple, ITimeSpanGrain>, ITimeSpanDirector
{
    public TimeSpanDirector(
        [PersistentState(ITimeSpanDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ITimeSpanGrain.Key, storeIds) { }
}
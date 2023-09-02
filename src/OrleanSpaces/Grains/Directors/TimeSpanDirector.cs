using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ITimeSpanDirector : IStoreDirector<TimeSpanTuple>, IGrainWithStringKey
{
    const string Key = "TimeSpanDirector";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanDirector : BaseDirector<TimeSpanTuple, ITimeSpanStore>, ITimeSpanDirector
{
    public TimeSpanDirector(
        [PersistentState(ITimeSpanDirector.Key, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ITimeSpanStore.Key, storeIds) { }
}
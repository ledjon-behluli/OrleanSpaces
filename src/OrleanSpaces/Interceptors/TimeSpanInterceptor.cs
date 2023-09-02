using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ITimeSpanInterceptor : IStoreInterceptor<TimeSpanTuple>, IGrainWithStringKey
{
    const string Key = "TimeSpanInterceptor";
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanInterceptor : BaseInterceptor<TimeSpanTuple, ITimeSpanGrain>, ITimeSpanInterceptor
{
    public TimeSpanInterceptor(
        [PersistentState(ITimeSpanInterceptor.Key, Constants.StorageName)]
        IPersistentState<HashSet<Guid>> storeIds)
        : base(ITimeSpanGrain.Key, storeIds) { }
}
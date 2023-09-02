using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface ITimeSpanInterceptor : IStoreInterceptor<TimeSpanTuple>, IGrainWithStringKey
{
    const string Key = "TimeSpanInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class TimeSpanInterceptor : BaseInterceptor<TimeSpanTuple, ITimeSpanGrain>, ITimeSpanInterceptor
{
    public TimeSpanInterceptor(
        [PersistentState(ITimeSpanInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(ITimeSpanGrain.Key, storeIds) { }
}
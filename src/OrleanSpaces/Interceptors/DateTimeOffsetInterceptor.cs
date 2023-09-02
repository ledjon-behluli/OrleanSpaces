using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IDateTimeOffsetInterceptor : IStoreInterceptor<DateTimeOffsetTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeOffsetInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DateTimeOffsetInterceptor : BaseInterceptor<DateTimeOffsetTuple, IDateTimeOffsetGrain>, IDateTimeOffsetInterceptor
{
    public DateTimeOffsetInterceptor(
        [PersistentState(IDateTimeOffsetInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDateTimeOffsetGrain.Key, storeIds) { }
}
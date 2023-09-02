using Orleans.Runtime;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Interceptors;

internal interface IDateTimeInterceptor : IStoreInterceptor<DateTimeTuple>, IGrainWithStringKey
{
    const string Key = "DateTimeInterceptor";
}

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DateTimeInterceptor : BaseInterceptor<DateTimeTuple, IDateTimeGrain>, IDateTimeInterceptor
{
    public DateTimeInterceptor(
        [PersistentState(IDateTimeInterceptor.Key, Constants.Store_StorageName)]
        IPersistentState<HashSet<string>> storeIds)
        : base(IDateTimeGrain.Key, storeIds) { }
}
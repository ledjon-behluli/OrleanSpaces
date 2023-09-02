using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DateTimeOffsetProcessor : BaseProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate, IDateTimeOffsetInterceptor>
{
    public DateTimeOffsetProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DateTimeOffsetTuple, DateTimeOffsetTemplate> router,
        ObserverChannel<DateTimeOffsetTuple> observerChannel,
        CallbackChannel<DateTimeOffsetTuple> callbackChannel)
        : base(IDateTimeOffsetGrain.Key, IDateTimeOffsetInterceptor.Key, options, client, router, observerChannel, callbackChannel)
    { }
}

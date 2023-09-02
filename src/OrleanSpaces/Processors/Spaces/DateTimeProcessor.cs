using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeProcessor : BaseProcessor<DateTimeTuple, DateTimeTemplate, IDateTimeInterceptor>
{
    public DateTimeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DateTimeTuple, DateTimeTemplate> router,
        ObserverChannel<DateTimeTuple> observerChannel,
        CallbackChannel<DateTimeTuple> callbackChannel)
        : base(IDateTimeGrain.Key, IDateTimeInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

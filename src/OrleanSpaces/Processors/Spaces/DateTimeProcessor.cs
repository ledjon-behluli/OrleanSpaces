using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeProcessor : BaseProcessor<DateTimeTuple, DateTimeTemplate, IDateTimeDirector>
{
    public DateTimeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DateTimeTuple, DateTimeTemplate> router,
        ObserverChannel<DateTimeTuple> observerChannel,
        CallbackChannel<DateTimeTuple> callbackChannel)
        : base(IDateTimeGrain.Key, IDateTimeDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

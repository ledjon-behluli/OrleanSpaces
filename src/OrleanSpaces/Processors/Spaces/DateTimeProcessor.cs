using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

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
        : base(Constants.RealmKey_DateTime, options, client, router, observerChannel, callbackChannel) { }
}

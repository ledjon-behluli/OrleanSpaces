using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetProcessor : BaseProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate, IDateTimeOffsetDirector>
{
    public DateTimeOffsetProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DateTimeOffsetTuple, DateTimeOffsetTemplate> router,
        ObserverChannel<DateTimeOffsetTuple> observerChannel,
        CallbackChannel<DateTimeOffsetTuple> callbackChannel)
        : base(Constants.RealmKey_DateTimeOffset, options, client, router, observerChannel, callbackChannel)
    { }
}

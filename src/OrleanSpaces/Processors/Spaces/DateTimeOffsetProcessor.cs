using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

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
        : base(IDateTimeOffsetGrain.Key, IDateTimeOffsetDirector.Key, options, client, router, observerChannel, callbackChannel)
    { }
}

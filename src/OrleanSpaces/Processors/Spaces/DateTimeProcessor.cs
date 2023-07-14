using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeProcessor : BaseProcessor<DateTimeTuple, DateTimeTemplate>
{
    public DateTimeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DateTimeTuple, DateTimeTemplate> router,
        ObserverChannel<DateTimeTuple> observerChannel,
        CallbackChannel<DateTimeTuple> callbackChannel)
        : base(IDateTimeGrain.Key, options, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IDateTimeGrain>(IDateTimeGrain.Key)) { }
}

using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanProcessor : BaseProcessor<TimeSpanTuple, TimeSpanTemplate, ITimeSpanDirector>
{
    public TimeSpanProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<TimeSpanTuple, TimeSpanTemplate> router,
        ObserverChannel<TimeSpanTuple> observerChannel,
        CallbackChannel<TimeSpanTuple> callbackChannel)
        : base(Constants.RealmKey_TimeSpan, options, client, router, observerChannel, callbackChannel) { }
}

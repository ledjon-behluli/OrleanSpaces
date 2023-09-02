using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

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
        : base(ITimeSpanGrain.Key, ITimeSpanDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

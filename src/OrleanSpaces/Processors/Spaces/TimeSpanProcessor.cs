using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class TimeSpanProcessor : BaseProcessor<TimeSpanTuple, TimeSpanTemplate, ITimeSpanInterceptor>
{
    public TimeSpanProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<TimeSpanTuple, TimeSpanTemplate> router,
        ObserverChannel<TimeSpanTuple> observerChannel,
        CallbackChannel<TimeSpanTuple> callbackChannel)
        : base(ITimeSpanGrain.Key, ITimeSpanInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

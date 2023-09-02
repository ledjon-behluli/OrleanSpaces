using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidProcessor : BaseProcessor<GuidTuple, GuidTemplate, IGuidInterceptor>
{
    public GuidProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<GuidTuple, GuidTemplate> router,
        ObserverChannel<GuidTuple> observerChannel,
        CallbackChannel<GuidTuple> callbackChannel)
        : base(IGuidGrain.Key, IGuidInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

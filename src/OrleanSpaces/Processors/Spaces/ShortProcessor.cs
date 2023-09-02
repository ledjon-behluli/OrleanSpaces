using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortProcessor : BaseProcessor<ShortTuple, ShortTemplate, IShortInterceptor>
{
    public ShortProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ShortTuple, ShortTemplate> router,
        ObserverChannel<ShortTuple> observerChannel,
        CallbackChannel<ShortTuple> callbackChannel)
        : base(IShortGrain.Key, IShortInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

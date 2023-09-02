using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatProcessor : BaseProcessor<FloatTuple, FloatTemplate, IFloatInterceptor>
{
    public FloatProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<FloatTuple, FloatTemplate> router,
        ObserverChannel<FloatTuple> observerChannel,
        CallbackChannel<FloatTuple> callbackChannel)
        : base(IFloatGrain.Key, IFloatInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

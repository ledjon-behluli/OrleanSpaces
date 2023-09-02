using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple, DoubleTemplate, IDoubleInterceptor>
{
    public DoubleProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DoubleTuple, DoubleTemplate> router,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(IDoubleGrain.Key, IDoubleInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

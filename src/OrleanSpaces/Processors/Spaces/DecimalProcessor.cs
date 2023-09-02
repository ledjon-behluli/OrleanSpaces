using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DecimalProcessor : BaseProcessor<DecimalTuple, DecimalTemplate, IDecimalInterceptor>
{
    public DecimalProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DecimalTuple, DecimalTemplate> router,
        ObserverChannel<DecimalTuple> observerChannel,
        CallbackChannel<DecimalTuple> callbackChannel)
        : base(IDecimalGrain.Key, IDecimalInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

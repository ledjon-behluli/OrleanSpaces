using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class UShortProcessor : BaseProcessor<UShortTuple, UShortTemplate, IUShortInterceptor>
{
    public UShortProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UShortTuple, UShortTemplate> router,
        ObserverChannel<UShortTuple> observerChannel,
        CallbackChannel<UShortTuple> callbackChannel)
        : base(IUShortGrain.Key, IUShortInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}
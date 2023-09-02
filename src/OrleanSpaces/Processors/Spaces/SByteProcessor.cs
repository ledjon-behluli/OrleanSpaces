using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple, SByteTemplate, ISByteInterceptor>
{
    public SByteProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<SByteTuple, SByteTemplate> router,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(ISByteGrain.Key, ISByteInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

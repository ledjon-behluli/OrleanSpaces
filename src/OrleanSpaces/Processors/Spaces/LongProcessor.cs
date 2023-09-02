using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class LongProcessor : BaseProcessor<LongTuple, LongTemplate, ILongInterceptor>
{
    public LongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<LongTuple, LongTemplate> router,
        ObserverChannel<LongTuple> observerChannel,
        CallbackChannel<LongTuple> callbackChannel)
        : base(ILongGrain.Key, ILongInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

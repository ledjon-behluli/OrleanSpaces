using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongProcessor : BaseProcessor<ULongTuple, ULongTemplate, IULongInterceptor>
{
    public ULongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ULongTuple, ULongTemplate> router,
        ObserverChannel<ULongTuple> observerChannel,
        CallbackChannel<ULongTuple> callbackChannel)
        : base(IULongGrain.Key, IULongInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}

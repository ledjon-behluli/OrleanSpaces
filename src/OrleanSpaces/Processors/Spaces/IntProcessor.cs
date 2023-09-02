using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class IntProcessor : BaseProcessor<IntTuple, IntTemplate, IIntInterceptor>
{
    public IntProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<IntTuple, IntTemplate> router,
        ObserverChannel<IntTuple> observerChannel,
        CallbackChannel<IntTuple> callbackChannel)
        : base(IIntGrain.Key, IIntInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}
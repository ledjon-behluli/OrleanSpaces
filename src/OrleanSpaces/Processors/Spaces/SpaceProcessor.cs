using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;
using OrleanSpaces.Channels;
using OrleanSpaces.Interceptors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class SpaceProcessor : BaseProcessor<SpaceTuple, SpaceTemplate, ISpaceInterceptor>
{
    public SpaceProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<SpaceTuple, SpaceTemplate> router,
        ObserverChannel<SpaceTuple> observerChannel,
        CallbackChannel<SpaceTuple> callbackChannel)
        : base(ISpaceGrain.Key, ISpaceInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}
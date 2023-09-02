using OrleanSpaces.Tuples;
using OrleanSpaces.Channels;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceProcessor : BaseProcessor<SpaceTuple, SpaceTemplate, ISpaceDirector>
{
    public SpaceProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<SpaceTuple, SpaceTemplate> router,
        ObserverChannel<SpaceTuple> observerChannel,
        CallbackChannel<SpaceTuple> callbackChannel)
        : base(ISpaceStore.Key, ISpaceDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;
using OrleanSpaces.Channels;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceProcessor : BaseProcessor<SpaceTuple, SpaceTemplate>
{
    public SpaceProcessor(
        IClusterClient client,
        ISpaceRouter<SpaceTuple, SpaceTemplate> router,
        ObserverChannel<SpaceTuple> observerChannel,
        CallbackChannel<SpaceTuple> callbackChannel)
        : base(ISpaceGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<ISpaceGrain>(ISpaceGrain.Key)) { }
}
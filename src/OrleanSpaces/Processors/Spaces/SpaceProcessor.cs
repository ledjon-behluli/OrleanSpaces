using OrleanSpaces.Tuples;
using OrleanSpaces.Grains;
using OrleanSpaces.Channels;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SpaceProcessor : BaseProcessor<SpaceTuple>
{
    public SpaceProcessor(
        IClusterClient client,
        IAgentProcessorBridge<SpaceTuple> bridge,
        ObserverChannel<SpaceTuple> observerChannel,
        CallbackChannel<SpaceTuple> callbackChannel)
        : base(ISpaceGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<ISpaceGrain>(ISpaceGrain.Key)) { }
}
using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidProcessor : BaseProcessor<GuidTuple>
{
    public GuidProcessor(
        IClusterClient client,
        IAgentProcessorBridge<GuidTuple> bridge,
        ObserverChannel<GuidTuple> observerChannel,
        CallbackChannel<GuidTuple> callbackChannel)
        : base(IGuidGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IGuidGrain>(IGuidGrain.Key)) { }
}

using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple>
{
    public SByteProcessor(
        IClusterClient client,
        IAgentProcessorBridge<SByteTuple> bridge,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(ISByteGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<ISByteGrain>(ISByteGrain.Key)) { }
}

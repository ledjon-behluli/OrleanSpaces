using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple>
{
    public DoubleProcessor(
        IClusterClient client,
        IAgentProcessorBridge<DoubleTuple> bridge,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(IDoubleGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IDoubleGrain>(IDoubleGrain.Key)) { }
}

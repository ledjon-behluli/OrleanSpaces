using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatProcessor : BaseProcessor<FloatTuple>
{
    public FloatProcessor(
        IClusterClient client,
        IAgentProcessorBridge<FloatTuple> bridge,
        ObserverChannel<FloatTuple> observerChannel,
        CallbackChannel<FloatTuple> callbackChannel)
        : base(IFloatGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IFloatGrain>(IFloatGrain.Key)) { }
}

using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalProcessor : BaseProcessor<DecimalTuple>
{
    public DecimalProcessor(
        IClusterClient client,
        IAgentProcessorBridge<DecimalTuple> bridge,
        ObserverChannel<DecimalTuple> observerChannel,
        CallbackChannel<DecimalTuple> callbackChannel)
        : base(IDecimalGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IDecimalGrain>(IDecimalGrain.Key)) { }
}

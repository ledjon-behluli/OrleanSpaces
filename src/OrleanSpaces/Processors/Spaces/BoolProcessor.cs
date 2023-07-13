using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolProcessor : BaseProcessor<BoolTuple>
{
    public BoolProcessor(
        IClusterClient client,
        IAgentProcessorBridge<BoolTuple> bridge,
        ObserverChannel<BoolTuple> observerChannel,
        CallbackChannel<BoolTuple> callbackChannel)
        : base(IBoolGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IBoolGrain>(IBoolGrain.Key)) { }
}
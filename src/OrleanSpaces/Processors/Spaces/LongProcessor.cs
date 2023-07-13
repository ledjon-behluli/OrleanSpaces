using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongProcessor : BaseProcessor<LongTuple>
{
    public LongProcessor(
        IClusterClient client,
        IAgentProcessorBridge<LongTuple> bridge,
        ObserverChannel<LongTuple> observerChannel,
        CallbackChannel<LongTuple> callbackChannel)
        : base(ILongGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<ILongGrain>(ILongGrain.Key)) { }
}

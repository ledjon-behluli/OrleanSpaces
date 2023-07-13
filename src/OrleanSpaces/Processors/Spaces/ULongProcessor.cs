using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongProcessor : BaseProcessor<ULongTuple>
{
    public ULongProcessor(
        IClusterClient client,
        IAgentProcessorBridge<ULongTuple> bridge,
        ObserverChannel<ULongTuple> observerChannel,
        CallbackChannel<ULongTuple> callbackChannel)
        : base(IULongGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IULongGrain>(IULongGrain.Key)) { }
}

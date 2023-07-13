using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortProcessor : BaseProcessor<UShortTuple>
{
    public UShortProcessor(
        IClusterClient client,
        IAgentProcessorBridge<UShortTuple> bridge,
        ObserverChannel<UShortTuple> observerChannel,
        CallbackChannel<UShortTuple> callbackChannel)
        : base(IUShortGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IUShortGrain>(IUShortGrain.Key)) { }
}
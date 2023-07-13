using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortProcessor : BaseProcessor<ShortTuple>
{
    public ShortProcessor(
        IClusterClient client,
        IAgentProcessorBridge<ShortTuple> bridge,
        ObserverChannel<ShortTuple> observerChannel,
        CallbackChannel<ShortTuple> callbackChannel)
        : base(IShortGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IShortGrain>(IShortGrain.Key)) { }
}

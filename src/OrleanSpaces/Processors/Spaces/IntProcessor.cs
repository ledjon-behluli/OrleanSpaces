using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntProcessor : BaseProcessor<IntTuple>
{
    public IntProcessor(
        IClusterClient client,
        IAgentProcessorBridge<IntTuple> bridge,
        ObserverChannel<IntTuple> observerChannel,
        CallbackChannel<IntTuple> callbackChannel)
        : base(IIntGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IIntGrain>(IIntGrain.Key)) { }
}
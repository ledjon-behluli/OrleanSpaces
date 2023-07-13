using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeProcessor : BaseProcessor<UHugeTuple>
{
    public UHugeProcessor(
        IClusterClient client,
        IAgentProcessorBridge<UHugeTuple> bridge,
        ObserverChannel<UHugeTuple> observerChannel,
        CallbackChannel<UHugeTuple> callbackChannel)
        : base(IUHugeGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IUHugeGrain>(IUHugeGrain.Key)) { }
}

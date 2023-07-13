using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeProcessor : BaseProcessor<HugeTuple>
{
    public HugeProcessor(
        IClusterClient client,
        IAgentProcessorBridge<HugeTuple> bridge,
        ObserverChannel<HugeTuple> observerChannel,
        CallbackChannel<HugeTuple> callbackChannel)
        : base(IHugeGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IHugeGrain>(IHugeGrain.Key)) { }
}
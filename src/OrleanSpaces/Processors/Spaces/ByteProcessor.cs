using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteProcessor : BaseProcessor<ByteTuple>
{
    public ByteProcessor(
        IClusterClient client,
        IAgentProcessorBridge<ByteTuple> bridge,
        ObserverChannel<ByteTuple> observerChannel,
        CallbackChannel<ByteTuple> callbackChannel)
        : base(IByteGrain.Key, client, bridge, observerChannel, callbackChannel,
             () => client.GetGrain<IByteGrain>(IByteGrain.Key)) { }
}
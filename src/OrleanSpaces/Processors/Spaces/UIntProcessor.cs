using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntProcessor : BaseProcessor<UIntTuple>
{
    public UIntProcessor(
        IClusterClient client,
        IAgentProcessorBridge<UIntTuple> bridge,
        ObserverChannel<UIntTuple> observerChannel,
        CallbackChannel<UIntTuple> callbackChannel)
        : base(IUIntGrain.Key, client, bridge, observerChannel, callbackChannel, 
            () => client.GetGrain<IUIntGrain>(IUIntGrain.Key)) { }
}

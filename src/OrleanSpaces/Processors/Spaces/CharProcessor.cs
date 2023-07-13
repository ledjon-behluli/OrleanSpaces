using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharProcessor : BaseProcessor<CharTuple>
{
    public CharProcessor(
        IClusterClient client,
        IAgentProcessorBridge<CharTuple> bridge,
        ObserverChannel<CharTuple> observerChannel,
        CallbackChannel<CharTuple> callbackChannel)
        : base(ICharGrain.Key, client, bridge, observerChannel, callbackChannel,
             () => client.GetGrain<ICharGrain>(ICharGrain.Key))
    { }
}

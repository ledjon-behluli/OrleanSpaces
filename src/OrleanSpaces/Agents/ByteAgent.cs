using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteAgent : BaseAgent<byte, ByteTuple, ByteTemplate>
{
    public ByteAgent(
        IClusterClient client,
        EvaluationChannel<ByteTuple> evaluationChannel,
        ObserverRegistry<ByteTuple> observerRegistry,
        CallbackRegistry<byte, ByteTuple, ByteTemplate> callbackRegistry)
        : base(client.GetGrain<IByteGrain>(IByteGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteAgent : Agent<byte, ByteTuple, ByteTemplate>
{
    public ByteAgent(
        IClusterClient client,
        EvaluationChannel<ByteTuple> evaluationChannel,
        ObserverChannel<ByteTuple> observerChannel,
        ObserverRegistry<ByteTuple> observerRegistry,
        CallbackChannel<ByteTuple> callbackChannel,
        CallbackRegistry<byte, ByteTuple, ByteTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class ByteAgentProvider : AgentProvider<byte, ByteTuple, ByteTemplate>
{
    public ByteAgentProvider(IClusterClient client, ByteAgent agent) :
        base(client.GetGrain<IByteGrain>(IByteGrain.Key), agent)
    { }
}
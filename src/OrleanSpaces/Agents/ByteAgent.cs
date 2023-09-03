using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteAgent : BaseAgent<byte, ByteTuple, ByteTemplate>
{
    public ByteAgent(
        SpaceClientOptions options,
        EvaluationChannel<ByteTuple> evaluationChannel,
        ObserverRegistry<ByteTuple> observerRegistry,
        CallbackRegistry<byte, ByteTuple, ByteTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
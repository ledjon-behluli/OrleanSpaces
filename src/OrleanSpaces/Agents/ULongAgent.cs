using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongAgent : BaseAgent<ulong, ULongTuple, ULongTemplate>
{
    public ULongAgent(
        SpaceClientOptions options,
        EvaluationChannel<ULongTuple> evaluationChannel,
        ObserverRegistry<ULongTuple> observerRegistry,
        CallbackRegistry<ulong, ULongTuple, ULongTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
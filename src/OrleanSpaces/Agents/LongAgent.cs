using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class LongAgent : BaseAgent<long, LongTuple, LongTemplate>
{
    public LongAgent(
        SpaceOptions options,
        EvaluationChannel<LongTuple> evaluationChannel,
        ObserverRegistry<LongTuple> observerRegistry,
        CallbackRegistry<long, LongTuple, LongTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
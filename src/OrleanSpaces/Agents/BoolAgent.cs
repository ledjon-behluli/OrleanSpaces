using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolAgent : BaseAgent<bool, BoolTuple, BoolTemplate>
{
    public BoolAgent(
        SpaceClientOptions options,
        EvaluationChannel<BoolTuple> evaluationChannel,
        ObserverRegistry<BoolTuple> observerRegistry,
        CallbackRegistry<bool, BoolTuple, BoolTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}

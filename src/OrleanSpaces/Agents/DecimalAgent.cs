using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalAgent : BaseAgent<decimal, DecimalTuple, DecimalTemplate>
{
    public DecimalAgent(
        SpaceOptions options,
        EvaluationChannel<DecimalTuple> evaluationChannel,
        ObserverRegistry<DecimalTuple> observerRegistry,
        CallbackRegistry<decimal, DecimalTuple, DecimalTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
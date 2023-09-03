using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleAgent : BaseAgent<double, DoubleTuple, DoubleTemplate>
{
    public DoubleAgent(
        SpaceClientOptions options,
        EvaluationChannel<DoubleTuple> evaluationChannel,
        ObserverRegistry<DoubleTuple> observerRegistry,
        CallbackRegistry<double, DoubleTuple, DoubleTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
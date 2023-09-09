using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatAgent : BaseAgent<float, FloatTuple, FloatTemplate>
{
    public FloatAgent(
        SpaceClientOptions options,
        EvaluationChannel<FloatTuple> evaluationChannel,
        ObserverRegistry<FloatTuple> observerRegistry,
        CallbackRegistry<float, FloatTuple, FloatTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
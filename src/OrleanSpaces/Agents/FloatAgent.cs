using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatAgent : Agent<float, FloatTuple, FloatTemplate>
{
    public FloatAgent(
        IClusterClient client,
        EvaluationChannel<FloatTuple> evaluationChannel,
        ObserverChannel<FloatTuple> observerChannel,
        ObserverRegistry<FloatTuple> observerRegistry,
        CallbackChannel<FloatTuple> callbackChannel,
        CallbackRegistry<float, FloatTuple, FloatTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class FloatAgentProvider : AgentProvider<float, FloatTuple, FloatTemplate>
{
    public FloatAgentProvider(IClusterClient client, FloatAgent agent) :
        base(client.GetGrain<IFloatGrain>(IFloatGrain.Key), agent)
    { }
}
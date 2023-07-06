using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleAgent : Agent<double, DoubleTuple, DoubleTemplate>
{
    public DoubleAgent(
        IClusterClient client,
        EvaluationChannel<DoubleTuple> evaluationChannel,
        ObserverChannel<DoubleTuple> observerChannel,
        ObserverRegistry<DoubleTuple> observerRegistry,
        CallbackChannel<DoubleTuple> callbackChannel,
        CallbackRegistry<double, DoubleTuple, DoubleTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class DoubleAgentProvider : AgentProvider<double, DoubleTuple, DoubleTemplate>
{
    public DoubleAgentProvider(IClusterClient client, DoubleAgent agent) :
        base(client.GetGrain<IDoubleGrain>(IDoubleGrain.Key), agent)
    { }
}
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolAgent : Agent<bool, BoolTuple, BoolTemplate>
{
    public BoolAgent(
        IClusterClient client,
        EvaluationChannel<BoolTuple> evaluationChannel,
        ObserverChannel<BoolTuple> observerChannel,
        ObserverRegistry<BoolTuple> observerRegistry,
        CallbackChannel<BoolTuple> callbackChannel,
        CallbackRegistry<bool, BoolTuple, BoolTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class BoolAgentProvider : AgentProvider<bool, BoolTuple, BoolTemplate>
{
    public BoolAgentProvider(IClusterClient client, BoolAgent agent) :
        base(client.GetGrain<IBoolGrain>(IBoolGrain.Key), agent)
    { }
}
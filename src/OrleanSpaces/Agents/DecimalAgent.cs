using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalAgent : Agent<decimal, DecimalTuple, DecimalTemplate>
{
    public DecimalAgent(
        IClusterClient client,
        EvaluationChannel<DecimalTuple> evaluationChannel,
        ObserverChannel<DecimalTuple> observerChannel,
        ObserverRegistry<DecimalTuple> observerRegistry,
        CallbackChannel<DecimalTuple> callbackChannel,
        CallbackRegistry<decimal, DecimalTuple, DecimalTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class DecimalAgentProvider : AgentProvider<decimal, DecimalTuple, DecimalTemplate>
{
    public DecimalAgentProvider(IClusterClient client, DecimalAgent agent) :
        base(client.GetGrain<IDecimalGrain>(IDecimalGrain.Key), agent)
    { }
}
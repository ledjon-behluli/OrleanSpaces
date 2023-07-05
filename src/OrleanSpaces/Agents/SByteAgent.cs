using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteAgent : Agent<sbyte, SByteTuple, SByteTemplate>
{
    public SByteAgent(
        IClusterClient client,
        EvaluationChannel<SByteTuple> evaluationChannel,
        ObserverChannel<SByteTuple> observerChannel,
        ObserverRegistry<SByteTuple> observerRegistry,
        CallbackChannel<SByteTuple> callbackChannel,
        CallbackRegistry<sbyte, SByteTuple, SByteTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class SByteAgentProvider : AgentProvider<sbyte, SByteTuple, SByteTemplate>
{
    public SByteAgentProvider(IClusterClient client, SByteAgent agent) :
        base(client.GetGrain<ISByteGrain>(ISByteGrain.Key), agent)
    { }
}
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeAgent : Agent<Int128, HugeTuple, HugeTemplate>
{
    public HugeAgent(
        IClusterClient client,
        EvaluationChannel<HugeTuple> evaluationChannel,
        ObserverChannel<HugeTuple> observerChannel,
        ObserverRegistry<HugeTuple> observerRegistry,
        CallbackChannel<HugeTuple> callbackChannel,
        CallbackRegistry<Int128, HugeTuple, HugeTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class HugeAgentProvider : AgentProvider<Int128, HugeTuple, HugeTemplate>
{
    public HugeAgentProvider(IClusterClient client, HugeAgent agent) :
        base(client.GetGrain<IHugeGrain>(IHugeGrain.Key), agent)
    { }
}
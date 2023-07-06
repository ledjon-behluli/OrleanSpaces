using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeAgent : Agent<UInt128, UHugeTuple, UHugeTemplate>
{
    public UHugeAgent(
        IClusterClient client,
        EvaluationChannel<UHugeTuple> evaluationChannel,
        ObserverChannel<UHugeTuple> observerChannel,
        ObserverRegistry<UHugeTuple> observerRegistry,
        CallbackChannel<UHugeTuple> callbackChannel,
        CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class UHugeAgentProvider : AgentProvider<UInt128, UHugeTuple, UHugeTemplate>
{
    public UHugeAgentProvider(IClusterClient client, UHugeAgent agent) :
        base(client.GetGrain<IUHugeGrain>(IUHugeGrain.Key), agent)
    { }
}
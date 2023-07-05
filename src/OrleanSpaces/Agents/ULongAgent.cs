using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongAgent : Agent<ulong, ULongTuple, ULongTemplate>
{
    public ULongAgent(
        IClusterClient client,
        EvaluationChannel<ULongTuple> evaluationChannel,
        ObserverChannel<ULongTuple> observerChannel,
        ObserverRegistry<ULongTuple> observerRegistry,
        CallbackChannel<ULongTuple> callbackChannel,
        CallbackRegistry<ulong, ULongTuple, ULongTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class ULongAgentProvider : AgentProvider<ulong, ULongTuple, ULongTemplate>
{
    public ULongAgentProvider(IClusterClient client, ULongAgent agent) :
        base(client.GetGrain<IULongGrain>(IULongGrain.Key), agent)
    { }
}
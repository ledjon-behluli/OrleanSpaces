using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntAgent : Agent<uint, UIntTuple, UIntTemplate>
{
    public UIntAgent(
        IClusterClient client,
        EvaluationChannel<UIntTuple> evaluationChannel,
        ObserverChannel<UIntTuple> observerChannel,
        ObserverRegistry<UIntTuple> observerRegistry,
        CallbackChannel<UIntTuple> callbackChannel,
        CallbackRegistry<uint, UIntTuple, UIntTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class UIntAgentProvider : AgentProvider<uint, UIntTuple, UIntTemplate>
{
    public UIntAgentProvider(IClusterClient client, UIntAgent agent) :
        base(client.GetGrain<IUIntGrain>(IUIntGrain.Key), agent)
    { }
}
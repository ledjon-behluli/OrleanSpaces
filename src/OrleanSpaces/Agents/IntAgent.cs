using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

internal sealed class IntAgentProvider : AgentProvider<int, IntTuple, IntTemplate>
{
    public IntAgentProvider(IClusterClient client, IntAgent agent) :
        base(client.GetGrain<IIntGrain>(IIntGrain.Key), agent) { }
}


[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntAgent : Agent<int, IntTuple, IntTemplate>
{
    public IntAgent(
        IClusterClient client,
        EvaluationChannel<IntTuple> evaluationChannel,
        ObserverChannel<IntTuple> observerChannel,
        ObserverRegistry<IntTuple> observerRegistry,
        CallbackChannel<IntTuple> callbackChannel,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

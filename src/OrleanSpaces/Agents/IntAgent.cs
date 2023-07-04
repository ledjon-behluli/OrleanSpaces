using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

internal sealed class IntAgentProvider : AgentProvider<int, IntTuple, IntTemplate, IntGrain>
{
    public IntAgentProvider(IClusterClient client, IntAgent agent) :
        base(client, agent) { }
}


[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntAgent : Agent<int, IntTuple, IntTemplate>
{
    public IntAgent(
        IClusterClient client,
        EvaluationChannel<IntTuple> evaluationChannel,
        ObserverChannel<IntTuple> observerChannel,
        ObserverRegistry<IntTuple> observerRegistry,
        CallbackChannel<IntTuple, IntTemplate> callbackChannel,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

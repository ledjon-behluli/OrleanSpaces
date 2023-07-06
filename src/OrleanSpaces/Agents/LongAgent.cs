using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongAgent : Agent<long, LongTuple, LongTemplate>
{
    public LongAgent(
        IClusterClient client,
        EvaluationChannel<LongTuple> evaluationChannel,
        ObserverChannel<LongTuple> observerChannel,
        ObserverRegistry<LongTuple> observerRegistry,
        CallbackChannel<LongTuple> callbackChannel,
        CallbackRegistry<long, LongTuple, LongTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class LongAgentProvider : AgentProvider<long, LongTuple, LongTemplate>
{
    public LongAgentProvider(IClusterClient client, LongAgent agent) :
        base(client.GetGrain<ILongGrain>(ILongGrain.Key), agent)
    { }
}
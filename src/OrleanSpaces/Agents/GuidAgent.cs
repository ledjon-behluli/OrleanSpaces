using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidAgent : Agent<Guid, GuidTuple, GuidTemplate>
{
    public GuidAgent(
        IClusterClient client,
        EvaluationChannel<GuidTuple> evaluationChannel,
        ObserverChannel<GuidTuple> observerChannel,
        ObserverRegistry<GuidTuple> observerRegistry,
        CallbackChannel<GuidTuple> callbackChannel,
        CallbackRegistry<Guid, GuidTuple, GuidTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class GuidAgentProvider : AgentProvider<Guid, GuidTuple, GuidTemplate>
{
    public GuidAgentProvider(IClusterClient client, GuidAgent agent) :
        base(client.GetGrain<IGuidGrain>(IGuidGrain.Key), agent)
    { }
}
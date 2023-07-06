using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortAgent : Agent<ushort, UShortTuple, UShortTemplate>
{
    public UShortAgent(
        IClusterClient client,
        EvaluationChannel<UShortTuple> evaluationChannel,
        ObserverChannel<UShortTuple> observerChannel,
        ObserverRegistry<UShortTuple> observerRegistry,
        CallbackChannel<UShortTuple> callbackChannel,
        CallbackRegistry<ushort, UShortTuple, UShortTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class UShortAgentProvider : AgentProvider<ushort, UShortTuple, UShortTemplate>
{
    public UShortAgentProvider(IClusterClient client, UShortAgent agent) :
        base(client.GetGrain<IUShortGrain>(IUShortGrain.Key), agent)
    { }
}
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortAgent : Agent<short, ShortTuple, ShortTemplate>
{
    public ShortAgent(
        IClusterClient client,
        EvaluationChannel<ShortTuple> evaluationChannel,
        ObserverChannel<ShortTuple> observerChannel,
        ObserverRegistry<ShortTuple> observerRegistry,
        CallbackChannel<ShortTuple> callbackChannel,
        CallbackRegistry<short, ShortTuple, ShortTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class ShortAgentProvider : AgentProvider<short, ShortTuple, ShortTemplate>
{
    public ShortAgentProvider(IClusterClient client, ShortAgent agent) :
        base(client.GetGrain<IShortGrain>(IShortGrain.Key), agent)
    { }
}
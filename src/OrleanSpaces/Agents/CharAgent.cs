using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharAgent : Agent<char, CharTuple, CharTemplate>
{
    public CharAgent(
        IClusterClient client,
        EvaluationChannel<CharTuple> evaluationChannel,
        ObserverChannel<CharTuple> observerChannel,
        ObserverRegistry<CharTuple> observerRegistry,
        CallbackChannel<CharTuple> callbackChannel,
        CallbackRegistry<char, CharTuple, CharTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class CharAgentProvider : AgentProvider<char, CharTuple, CharTemplate>
{
    public CharAgentProvider(IClusterClient client, CharAgent agent) :
        base(client.GetGrain<ICharGrain>(ICharGrain.Key), agent)
    { }
}
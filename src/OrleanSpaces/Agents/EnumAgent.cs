using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal class EnumAgent<T> : Agent<T, EnumTuple<T>, EnumTemplate<T>>
    where T : unmanaged, Enum
{
    public EnumAgent(
        IClusterClient client,
        EvaluationChannel<EnumTuple<T>> evaluationChannel,
        ObserverChannel<EnumTuple<T>> observerChannel,
        ObserverRegistry<EnumTuple<T>> observerRegistry,
        CallbackChannel<EnumTuple<T>> callbackChannel,
        CallbackRegistry<T, EnumTuple<T>, EnumTemplate<T>> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal class EnumAgentProvider<T> : AgentProvider<T, EnumTuple<T>, EnumTemplate<T>>
    where T : unmanaged, Enum
{
    public EnumAgentProvider(IClusterClient client, EnumAgent<T> agent) :
        base(client.GetGrain<IEnumGrain>(IEnumGrain.Key), agent)
    { }
}
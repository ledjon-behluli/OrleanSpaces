using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongAgent : BaseAgent<ulong, ULongTuple, ULongTemplate>
{
    public ULongAgent(
        IClusterClient client,
        EvaluationChannel<ULongTuple> evaluationChannel,
        ObserverRegistry<ULongTuple> observerRegistry,
        CallbackRegistry<ulong, ULongTuple, ULongTemplate> callbackRegistry)
        : base(client.GetGrain<IULongGrain>(IULongGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongAgent : BaseAgent<long, LongTuple, LongTemplate>
{
    public LongAgent(
        IClusterClient client,
        EvaluationChannel<LongTuple> evaluationChannel,
        ObserverRegistry<LongTuple> observerRegistry,
        CallbackRegistry<long, LongTuple, LongTemplate> callbackRegistry)
        : base(client.GetGrain<ILongGrain>(ILongGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
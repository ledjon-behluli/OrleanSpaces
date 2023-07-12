using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolAgent : BaseAgent<bool, BoolTuple, BoolTemplate>
{
    public BoolAgent(
        IClusterClient client,
        EvaluationChannel<BoolTuple> evaluationChannel,
        ObserverRegistry<BoolTuple> observerRegistry,
        CallbackRegistry<bool, BoolTuple, BoolTemplate> callbackRegistry)
        : base(client.GetGrain<IBoolGrain>(IBoolGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}

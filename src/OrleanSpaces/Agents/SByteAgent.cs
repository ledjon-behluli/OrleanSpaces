using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteAgent : BaseAgent<sbyte, SByteTuple, SByteTemplate>
{
    public SByteAgent(
        IClusterClient client,
        EvaluationChannel<SByteTuple> evaluationChannel,
        ObserverRegistry<SByteTuple> observerRegistry,
        CallbackRegistry<sbyte, SByteTuple, SByteTemplate> callbackRegistry)
        : base(client.GetGrain<ISByteGrain>(ISByteGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
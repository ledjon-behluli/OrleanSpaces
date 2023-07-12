using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidAgent : BaseAgent<Guid, GuidTuple, GuidTemplate>
{
    public GuidAgent(
        IClusterClient client,
        EvaluationChannel<GuidTuple> evaluationChannel,
        ObserverRegistry<GuidTuple> observerRegistry,
        CallbackRegistry<Guid, GuidTuple, GuidTemplate> callbackRegistry)
        : base(client.GetGrain<IGuidGrain>(IGuidGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
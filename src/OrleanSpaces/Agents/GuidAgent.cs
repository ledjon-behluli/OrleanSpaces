using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidAgent : BaseAgent<Guid, GuidTuple, GuidTemplate>
{
    public GuidAgent(
        SpaceClientOptions options,
        EvaluationChannel<GuidTuple> evaluationChannel,
        ObserverRegistry<GuidTuple> observerRegistry,
        CallbackRegistry<Guid, GuidTuple, GuidTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class TimeSpanAgent : BaseAgent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>
{
    public TimeSpanAgent(
        SpaceOptions options,
        EvaluationChannel<TimeSpanTuple> evaluationChannel,
        ObserverRegistry<TimeSpanTuple> observerRegistry,
        CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
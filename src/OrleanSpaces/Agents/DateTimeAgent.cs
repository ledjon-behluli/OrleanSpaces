using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DateTimeAgent : BaseAgent<DateTime, DateTimeTuple, DateTimeTemplate>
{
    public DateTimeAgent(
        SpaceOptions options,
        EvaluationChannel<DateTimeTuple> evaluationChannel,
        ObserverRegistry<DateTimeTuple> observerRegistry,
        CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
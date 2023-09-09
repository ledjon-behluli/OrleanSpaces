using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeAgent : BaseAgent<DateTime, DateTimeTuple, DateTimeTemplate>
{
    public DateTimeAgent(
        SpaceClientOptions options,
        EvaluationChannel<DateTimeTuple> evaluationChannel,
        ObserverRegistry<DateTimeTuple> observerRegistry,
        CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
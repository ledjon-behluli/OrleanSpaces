using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetAgent : BaseAgent<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>
{
    public DateTimeOffsetAgent(
        SpaceOptions options,
        EvaluationChannel<DateTimeOffsetTuple> evaluationChannel,
        ObserverRegistry<DateTimeOffsetTuple> observerRegistry,
        CallbackRegistry<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
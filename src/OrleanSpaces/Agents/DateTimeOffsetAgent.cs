using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetAgent : Agent<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>
{
    public DateTimeOffsetAgent(
        IClusterClient client,
        EvaluationChannel<DateTimeOffsetTuple> evaluationChannel,
        ObserverChannel<DateTimeOffsetTuple> observerChannel,
        ObserverRegistry<DateTimeOffsetTuple> observerRegistry,
        CallbackChannel<DateTimeOffsetTuple> callbackChannel,
        CallbackRegistry<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class DateTimeOffsetAgentProvider : AgentProvider<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>
{
    public DateTimeOffsetAgentProvider(IClusterClient client, DateTimeOffsetAgent agent) :
        base(client.GetGrain<IDateTimeOffsetGrain>(IDateTimeOffsetGrain.Key), agent)
    { }
}
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeAgent : Agent<DateTime, DateTimeTuple, DateTimeTemplate>
{
    public DateTimeAgent(
        IClusterClient client,
        EvaluationChannel<DateTimeTuple> evaluationChannel,
        ObserverChannel<DateTimeTuple> observerChannel,
        ObserverRegistry<DateTimeTuple> observerRegistry,
        CallbackChannel<DateTimeTuple> callbackChannel,
        CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class DateTimeAgentProvider : AgentProvider<DateTime, DateTimeTuple, DateTimeTemplate>
{
    public DateTimeAgentProvider(IClusterClient client, DateTimeAgent agent) :
        base(client.GetGrain<IDateTimeGrain>(IDateTimeGrain.Key), agent)
    { }
}
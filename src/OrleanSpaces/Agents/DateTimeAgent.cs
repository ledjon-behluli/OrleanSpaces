using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeAgent : BaseAgent<DateTime, DateTimeTuple, DateTimeTemplate>
{
    public DateTimeAgent(
        IClusterClient client,
        EvaluationChannel<DateTimeTuple> evaluationChannel,
        ObserverRegistry<DateTimeTuple> observerRegistry,
        CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate> callbackRegistry)
        : base(client.GetGrain<IDateTimeGrain>(IDateTimeGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
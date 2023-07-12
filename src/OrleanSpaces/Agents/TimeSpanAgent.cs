using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanAgent : BaseAgent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>
{
    public TimeSpanAgent(
        IClusterClient client,
        EvaluationChannel<TimeSpanTuple> evaluationChannel,
        ObserverRegistry<TimeSpanTuple> observerRegistry,
        CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate> callbackRegistry)
        : base(client.GetGrain<ITimeSpanGrain>(ITimeSpanGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
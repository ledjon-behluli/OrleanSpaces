using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanAgent : Agent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>
{
    public TimeSpanAgent(
        IClusterClient client,
        EvaluationChannel<TimeSpanTuple> evaluationChannel,
        ObserverChannel<TimeSpanTuple> observerChannel,
        ObserverRegistry<TimeSpanTuple> observerRegistry,
        CallbackChannel<TimeSpanTuple> callbackChannel,
        CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate> callbackRegistry)
        : base(client, evaluationChannel, observerChannel, observerRegistry, callbackChannel, callbackRegistry) { }
}

internal sealed class TimeSpanAgentProvider : AgentProvider<TimeSpan, TimeSpanTuple, TimeSpanTemplate>
{
    public TimeSpanAgentProvider(IClusterClient client, TimeSpanAgent agent) :
        base(client.GetGrain<ITimeSpanGrain>(ITimeSpanGrain.Key), agent)
    { }
}
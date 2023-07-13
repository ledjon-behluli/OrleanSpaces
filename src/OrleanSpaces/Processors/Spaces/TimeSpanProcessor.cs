using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanProcessor : BaseProcessor<TimeSpanTuple>
{
    public TimeSpanProcessor(
        IClusterClient client,
        IAgentProcessorBridge<TimeSpanTuple> bridge,
        ObserverChannel<TimeSpanTuple> observerChannel,
        CallbackChannel<TimeSpanTuple> callbackChannel)
        : base(ITimeSpanGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<ITimeSpanGrain>(ITimeSpanGrain.Key)) { }
}

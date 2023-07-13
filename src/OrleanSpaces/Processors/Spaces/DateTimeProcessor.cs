using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeProcessor : BaseProcessor<DateTimeTuple>
{
    public DateTimeProcessor(
        IClusterClient client,
        IAgentProcessorBridge<DateTimeTuple> bridge,
        ObserverChannel<DateTimeTuple> observerChannel,
        CallbackChannel<DateTimeTuple> callbackChannel)
        : base(IDateTimeGrain.Key, client, bridge, observerChannel, callbackChannel,
            () => client.GetGrain<IDateTimeGrain>(IDateTimeGrain.Key)) { }
}

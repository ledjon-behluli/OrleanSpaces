using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeOffsetProcessor : BaseProcessor<DateTimeOffsetTuple>
{
    public DateTimeOffsetProcessor(
        IClusterClient client,
        IAgentProcessorBridge<DateTimeOffsetTuple> bridge,
        ObserverChannel<DateTimeOffsetTuple> observerChannel,
        CallbackChannel<DateTimeOffsetTuple> callbackChannel)
        : base(IDateTimeOffsetGrain.Key, client, bridge, observerChannel, callbackChannel,
             () => client.GetGrain<IDateTimeOffsetGrain>(IDateTimeOffsetGrain.Key))
    { }
}

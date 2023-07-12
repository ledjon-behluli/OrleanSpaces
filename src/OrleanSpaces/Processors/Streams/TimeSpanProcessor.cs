using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Streams;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class TimeSpanProcessor : BaseProcessor<TimeSpanTuple>
{
    public TimeSpanProcessor(
        IClusterClient client,
        ITupleActionReceiver<TimeSpanTuple> receiver,
        ObserverChannel<TimeSpanTuple> observerChannel,
        CallbackChannel<TimeSpanTuple> callbackChannel)
        : base(ITimeSpanGrain.Key, client, receiver, observerChannel, callbackChannel) { }
}

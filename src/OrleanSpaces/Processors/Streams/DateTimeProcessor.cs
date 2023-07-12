using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Streams;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeProcessor : BaseProcessor<DateTimeTuple>
{
    public DateTimeProcessor(
        IClusterClient client,
        ITupleActionReceiver<DateTimeTuple> receiver,
        ObserverChannel<DateTimeTuple> observerChannel,
        CallbackChannel<DateTimeTuple> callbackChannel)
        : base(IDateTimeGrain.Key, client, receiver, observerChannel, callbackChannel) { }
}

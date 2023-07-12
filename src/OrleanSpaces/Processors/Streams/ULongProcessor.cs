using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Streams;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongProcessor : BaseProcessor<ULongTuple>
{
    public ULongProcessor(
        IClusterClient client,
        ITupleActionReceiver<ULongTuple> receiver,
        ObserverChannel<ULongTuple> observerChannel,
        CallbackChannel<ULongTuple> callbackChannel)
        : base(IULongGrain.Key, client, receiver, observerChannel, callbackChannel) { }
}

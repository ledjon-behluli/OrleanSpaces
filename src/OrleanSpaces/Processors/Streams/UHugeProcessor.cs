using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Streams;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeProcessor : BaseProcessor<UHugeTuple>
{
    public UHugeProcessor(
        IClusterClient client,
        ITupleActionReceiver<UHugeTuple> receiver,
        ObserverChannel<UHugeTuple> observerChannel,
        CallbackChannel<UHugeTuple> callbackChannel)
        : base(IUHugeGrain.Key, client, receiver, observerChannel, callbackChannel) { }
}

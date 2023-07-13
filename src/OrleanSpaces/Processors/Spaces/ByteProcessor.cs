using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteProcessor : BaseProcessor<ByteTuple, ByteTemplate>
{
    public ByteProcessor(
        IClusterClient client,
        ISpaceRouter<ByteTuple, ByteTemplate> router,
        ObserverChannel<ByteTuple> observerChannel,
        CallbackChannel<ByteTuple> callbackChannel)
        : base(IByteGrain.Key, client, router, observerChannel, callbackChannel,
             () => client.GetGrain<IByteGrain>(IByteGrain.Key)) { }
}
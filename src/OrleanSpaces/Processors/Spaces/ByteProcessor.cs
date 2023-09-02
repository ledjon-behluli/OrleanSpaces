using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteProcessor : BaseProcessor<ByteTuple, ByteTemplate, IByteDirector>
{
    public ByteProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ByteTuple, ByteTemplate> router,
        ObserverChannel<ByteTuple> observerChannel,
        CallbackChannel<ByteTuple> callbackChannel)
        : base(IByteGrain.Key, IByteDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
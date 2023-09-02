using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

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
        : base(IByteStore.Key, IByteDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
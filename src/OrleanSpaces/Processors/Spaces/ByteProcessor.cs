using OrleanSpaces.Channels;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ByteProcessor : BaseProcessor<ByteTuple, ByteTemplate>
{
    public ByteProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ByteTuple, ByteTemplate> router,
        ObserverChannel<ByteTuple> observerChannel,
        CallbackChannel<ByteTuple> callbackChannel)
        : base(IByteInterceptor.Key, options, client, router, observerChannel, callbackChannel,
             () => client.GetGrain<IByteInterceptor>(IByteInterceptor.Key)) { }
}
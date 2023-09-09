using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple, DoubleTemplate, IDoubleDirector>
{
    public DoubleProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<DoubleTuple, DoubleTemplate> router,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(Constants.RealmKey_Double, options, client, router, observerChannel, callbackChannel) { }
}

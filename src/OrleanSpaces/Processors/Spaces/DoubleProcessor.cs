using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple, DoubleTemplate, IDoubleDirector>
{
    public DoubleProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DoubleTuple, DoubleTemplate> router,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(IDoubleStore.Key, IDoubleDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

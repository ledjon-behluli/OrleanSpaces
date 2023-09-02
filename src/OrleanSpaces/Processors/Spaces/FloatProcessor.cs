using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatProcessor : BaseProcessor<FloatTuple, FloatTemplate, IFloatDirector>
{
    public FloatProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<FloatTuple, FloatTemplate> router,
        ObserverChannel<FloatTuple> observerChannel,
        CallbackChannel<FloatTuple> callbackChannel)
        : base(IFloatStore.Key, IFloatDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

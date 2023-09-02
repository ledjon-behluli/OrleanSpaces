using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeProcessor : BaseProcessor<HugeTuple, HugeTemplate, IHugeDirector>
{
    public HugeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<HugeTuple, HugeTemplate> router,
        ObserverChannel<HugeTuple> observerChannel,
        CallbackChannel<HugeTuple> callbackChannel)
        : base(IHugeStore.Key, IHugeDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
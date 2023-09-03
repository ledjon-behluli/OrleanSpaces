using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeProcessor : BaseProcessor<HugeTuple, HugeTemplate, IHugeDirector>
{
    public HugeProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<HugeTuple, HugeTemplate> router,
        ObserverChannel<HugeTuple> observerChannel,
        CallbackChannel<HugeTuple> callbackChannel)
        : base(Constants.RealmKey_Huge, options, client, router, observerChannel, callbackChannel) { }
}
using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeProcessor : BaseProcessor<UHugeTuple, UHugeTemplate, IUHugeDirector>
{
    public UHugeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UHugeTuple, UHugeTemplate> router,
        ObserverChannel<UHugeTuple> observerChannel,
        CallbackChannel<UHugeTuple> callbackChannel)
        : base(Constants.RealmKey_UHuge, options, client, router, observerChannel, callbackChannel) { }
}

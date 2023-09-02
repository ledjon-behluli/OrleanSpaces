using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

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
        : base(IUHugeGrain.Key, IUHugeDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortProcessor : BaseProcessor<UShortTuple, UShortTemplate, IUShortDirector>
{
    public UShortProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UShortTuple, UShortTemplate> router,
        ObserverChannel<UShortTuple> observerChannel,
        CallbackChannel<UShortTuple> callbackChannel)
        : base(IUShortGrain.Key, IUShortDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
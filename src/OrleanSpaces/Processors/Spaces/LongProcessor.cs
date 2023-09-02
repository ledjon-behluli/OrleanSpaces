using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongProcessor : BaseProcessor<LongTuple, LongTemplate, ILongDirector>
{
    public LongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<LongTuple, LongTemplate> router,
        ObserverChannel<LongTuple> observerChannel,
        CallbackChannel<LongTuple> callbackChannel)
        : base(ILongGrain.Key, ILongDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}

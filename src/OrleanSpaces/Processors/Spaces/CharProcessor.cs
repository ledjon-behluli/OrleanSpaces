using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharProcessor : BaseProcessor<CharTuple, CharTemplate, ICharDirector>
{
    public CharProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<CharTuple, CharTemplate> router,
        ObserverChannel<CharTuple> observerChannel,
        CallbackChannel<CharTuple> callbackChannel)
        : base(ICharGrain.Key, ICharDirector.Key, options, client, router, observerChannel, callbackChannel)
    { }
}

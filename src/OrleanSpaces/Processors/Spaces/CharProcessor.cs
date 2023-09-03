using OrleanSpaces.Channels;
using OrleanSpaces.Grains.Directors;
using OrleanSpaces.Tuples.Specialized;

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
        : base(Constants.RealmKey_Char, options, client, router, observerChannel, callbackChannel)
    { }
}

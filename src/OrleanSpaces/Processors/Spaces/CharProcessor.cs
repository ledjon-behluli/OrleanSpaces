using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharProcessor : BaseProcessor<CharTuple, CharTemplate>
{
    public CharProcessor(
        IClusterClient client,
        ISpaceRouter<CharTuple, CharTemplate> router,
        ObserverChannel<CharTuple> observerChannel,
        CallbackChannel<CharTuple> callbackChannel)
        : base(ICharGrain.Key, client, router, observerChannel, callbackChannel,
             () => client.GetGrain<ICharGrain>(ICharGrain.Key))
    { }
}

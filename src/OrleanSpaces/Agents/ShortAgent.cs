using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortAgent : BaseAgent<short, ShortTuple, ShortTemplate>
{
    public ShortAgent(
        IClusterClient client,
        EvaluationChannel<ShortTuple> evaluationChannel,
        ObserverRegistry<ShortTuple> observerRegistry,
        CallbackRegistry<short, ShortTuple, ShortTemplate> callbackRegistry)
        : base(client.GetGrain<IShortGrain>(IShortGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}
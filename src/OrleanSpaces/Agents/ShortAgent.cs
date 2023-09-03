using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortAgent : BaseAgent<short, ShortTuple, ShortTemplate>
{
    public ShortAgent(
        SpaceClientOptions options,
        EvaluationChannel<ShortTuple> evaluationChannel,
        ObserverRegistry<ShortTuple> observerRegistry,
        CallbackRegistry<short, ShortTuple, ShortTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
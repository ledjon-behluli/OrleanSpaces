using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortAgent : BaseAgent<ushort, UShortTuple, UShortTemplate>
{
    public UShortAgent(
        SpaceOptions options,
        EvaluationChannel<UShortTuple> evaluationChannel,
        ObserverRegistry<UShortTuple> observerRegistry,
        CallbackRegistry<ushort, UShortTuple, UShortTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
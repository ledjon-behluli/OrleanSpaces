using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntAgent : BaseAgent<uint, UIntTuple, UIntTemplate>
{
    public UIntAgent(
        SpaceOptions options,
        EvaluationChannel<UIntTuple> evaluationChannel,
        ObserverRegistry<UIntTuple> observerRegistry,
        CallbackRegistry<uint, UIntTuple, UIntTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
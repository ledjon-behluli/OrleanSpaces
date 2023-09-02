using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class IntAgent : BaseAgent<int, IntTuple, IntTemplate>
{
    public IntAgent(
        SpaceOptions options,
        EvaluationChannel<IntTuple> evaluationChannel,
        ObserverRegistry<IntTuple> observerRegistry,
        CallbackRegistry<int, IntTuple, IntTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
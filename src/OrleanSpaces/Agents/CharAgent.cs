using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class CharAgent : BaseAgent<char, CharTuple, CharTemplate>
{
    public CharAgent(
        SpaceOptions options,
        EvaluationChannel<CharTuple> evaluationChannel,
        ObserverRegistry<CharTuple> observerRegistry,
        CallbackRegistry<char, CharTuple, CharTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
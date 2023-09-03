using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharAgent : BaseAgent<char, CharTuple, CharTemplate>
{
    public CharAgent(
        SpaceClientOptions options,
        EvaluationChannel<CharTuple> evaluationChannel,
        ObserverRegistry<CharTuple> observerRegistry,
        CallbackRegistry<char, CharTuple, CharTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}
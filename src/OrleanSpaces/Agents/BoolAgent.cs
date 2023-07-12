using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolAgent : Agent<bool, BoolTuple, BoolTemplate>
{
    public BoolAgent(
        IClusterClient client,
        EvaluationChannel<BoolTuple> evaluationChannel,
        ObserverRegistry<BoolTuple> observerRegistry,
        CallbackRegistry<bool, BoolTuple, BoolTemplate> callbackRegistry)
        : base(client.GetGrain<IBoolGrain>(IBoolGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolStreamProcessor : StreamProcessor<BoolTuple>
{
    public BoolStreamProcessor(
        IClusterClient client,
        ITupleActionReceiver<BoolTuple> receiver,
        ObserverChannel<BoolTuple> observerChannel,
        CallbackChannel<BoolTuple> callbackChannel)
        : base(IBoolGrain.Key, client, receiver, observerChannel, callbackChannel) { }
}
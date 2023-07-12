using Microsoft.Extensions.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors.Streams;

internal class BaseProcessor<TTuple> : BackgroundService, IAsyncObserver<TupleAction<TTuple>>
    where TTuple : ISpaceTuple
{
    private readonly string key;
    private readonly IClusterClient client;
    private readonly ITupleActionReceiver<TTuple> receiver;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly CallbackChannel<TTuple> callbackChannel;

    public BaseProcessor(
        string key,
        IClusterClient client,
        ITupleActionReceiver<TTuple> receiver,
        ObserverChannel<TTuple> observerChannel,
        CallbackChannel<TTuple> callbackChannel)
    {
        this.key = key ?? throw new ArgumentNullException(nameof(key));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await client.SubscribeAsync(this, StreamId.Create(Constants.StreamName, key));
        while (!cancellationToken.IsCancellationRequested)
        {

        }
    }

    public async Task OnNextAsync(TupleAction<TTuple> action, StreamSequenceToken? token = null)
    {
        await observerChannel.Writer.WriteAsync(action);

        switch (action.Type)
        {
            case TupleActionType.Insert:
                {
                    receiver.Add(action);
                    await callbackChannel.Writer.WriteAsync(action.Tuple);
                }
                break;
            case TupleActionType.Remove:
                receiver.Remove(action);
                break;
            case TupleActionType.Clear:
                receiver.Clear(action);
                break;
            default:
                throw new NotSupportedException();
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;
}
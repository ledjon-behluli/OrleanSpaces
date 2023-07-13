using Microsoft.Extensions.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors.Spaces;

internal class BaseProcessor<TTuple> : BackgroundService, IAsyncObserver<TupleAction<TTuple>>
    where TTuple : ISpaceTuple
{
    private readonly string key;
    private readonly IClusterClient client;
    private readonly IAgentProcessorBridge<TTuple> bridge;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly CallbackChannel<TTuple> callbackChannel;
    private readonly Func<ITupleStore<TTuple>> storeFactory;

    public BaseProcessor(
        string key,
        IClusterClient client,
        IAgentProcessorBridge<TTuple> bridge,
        ObserverChannel<TTuple> observerChannel,
        CallbackChannel<TTuple> callbackChannel,
        Func<ITupleStore<TTuple>> storeFactory)
    {
        this.key = key ?? throw new ArgumentNullException(nameof(key));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.storeFactory = storeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        bridge.SetStore(storeFactory());
        await client.SubscribeAsync(this, StreamId.Create(Constants.StreamName, key));
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) { }
        return Task.CompletedTask;
    }

    public async Task OnNextAsync(TupleAction<TTuple> action, StreamSequenceToken? token = null)
    {
        await bridge.ConsumeAsync(action);
        await observerChannel.Writer.WriteAsync(action);

        if (action.Type == TupleActionType.Insert)
        {
            await callbackChannel.Writer.WriteAsync(action.Tuple);
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;
}
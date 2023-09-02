using Microsoft.Extensions.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors.Spaces;

internal class BaseProcessor<TTuple, TTemplate, TInterceptor> : BackgroundService, IAsyncObserver<TupleAction<TTuple>>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
    where TInterceptor : IStoreInterceptor<TTuple>, IGrainWithStringKey
{
    private readonly string storeKey;
    private readonly string interceptorKey;
    private readonly SpaceOptions options;
    private readonly IClusterClient client;
    private readonly ISpaceRouter<TTuple, TTemplate> router;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly CallbackChannel<TTuple> callbackChannel;

    public BaseProcessor(
        string storeKey,
        string interceptorKey,
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<TTuple, TTemplate> router,
        ObserverChannel<TTuple> observerChannel,
        CallbackChannel<TTuple> callbackChannel)
    {
        this.storeKey = storeKey ?? throw new ArgumentNullException(nameof(storeKey));
        this.interceptorKey = interceptorKey ?? throw new ArgumentNullException(nameof(interceptorKey));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        router.RouteInterceptor(client.GetGrain<TInterceptor>(interceptorKey));

        var stream = client
            .GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<TTuple>>(StreamId.Create(Constants.StreamName, storeKey));

        await stream.SubscribeOrResumeAsync(this);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) { }
        return Task.CompletedTask;
    }

    public async Task OnNextAsync(TupleAction<TTuple> action, StreamSequenceToken? token = null)
    {
        await router.RouteAction(action);

        if (options.SubscribeToSelfGeneratedTuples)
        {
            await observerChannel.Writer.WriteAsync(action);
        }

        if (action.Type == TupleActionType.Insert)
        {
            await callbackChannel.Writer.WriteAsync(action.Address.Tuple);
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;
}
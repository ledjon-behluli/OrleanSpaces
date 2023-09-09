using Microsoft.Extensions.Hosting;
using Orleans.Runtime;
using Orleans.Streams;
using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors.Spaces;

internal class BaseProcessor<TTuple, TTemplate, TDirector> : BackgroundService, IAsyncObserver<TupleAction<TTuple>>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
    where TDirector : IStoreDirector<TTuple>, IGrainWithStringKey
{
    private readonly string realmKey;
    private readonly SpaceClientOptions options;
    private readonly IClusterClient client;
    private readonly ISpaceRouter<TTuple, TTemplate> router;
    private readonly ObserverChannel<TTuple> observerChannel;
    private readonly CallbackChannel<TTuple> callbackChannel;

    public BaseProcessor(
        string realmKey,
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<TTuple, TTemplate> router,
        ObserverChannel<TTuple> observerChannel,
        CallbackChannel<TTuple> callbackChannel)
    {
        this.realmKey = realmKey ?? throw new ArgumentNullException(nameof(realmKey));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
        this.observerChannel = observerChannel ?? throw new ArgumentNullException(nameof(observerChannel));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await router.RouteDirector(client.GetGrain<TDirector>(realmKey));

        var stream = client.GetStreamProvider(Constants.PubSubProvider)
            .GetStream<TupleAction<TTuple>>(StreamId.Create(Constants.StreamName, realmKey));

        var handles = await stream.GetAllSubscriptionHandles();
        await (handles.Count > 0 ? handles[0].ResumeAsync(this) : stream.SubscribeAsync(this));
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
            await callbackChannel.Writer.WriteAsync(action.StoreTuple.Tuple);
        }
    }

    Task IAsyncObserver<TupleAction<TTuple>>.OnCompletedAsync() => Task.CompletedTask;
    Task IAsyncObserver<TupleAction<TTuple>>.OnErrorAsync(Exception e) => Task.CompletedTask;
}
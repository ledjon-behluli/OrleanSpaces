using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Grains;
using OrleanSpaces.Observers;
using Microsoft.Extensions.Hosting;

namespace OrleanSpaces;

internal partial class SpaceAgent
{
    private readonly IClusterClient client;
    private readonly ICallbackRegistry callbackRegistry;
    private readonly IObserverRegistry observerRegistry;
    private readonly ILogger<SpaceAgent> logger;

    protected ISpaceGrain SpaceGrain => client.GetGrain<ISpaceGrain>(Guid.Empty);

    private static readonly TaskFactory taskFactory =
      new(CancellationToken.None,
          TaskCreationOptions.None,
          TaskContinuationOptions.None,
          TaskScheduler.Default);

    public SpaceAgent(
        IClusterClient client,
        IHostApplicationLifetime lifetime,
        ICallbackRegistry callbackRegistry,
        IObserverRegistry observerRegistry,
        ILogger<SpaceAgent> logger)
    {
        if (lifetime == null) 
            throw new ArgumentNullException(nameof(lifetime));
       
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        lifetime.ApplicationStarted.Register(() =>
        {
            OpenClusterConnection();
            EstablishStreamConnection();
        });
        lifetime.ApplicationStopping.Register(CloseClusterConnection());
    }

    private static void RunSync(Func<Task> func) =>
        taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();

    private Action OpenClusterConnection()
    {
        return () => RunSync(async () =>
        {
            if (!client.IsInitialized)
            {
                logger.LogDebug("Opening connection to cluster.");

                await client.Connect();

                logger.LogDebug("Opening connection established.");
            }
        });
    }
    
    private Action EstablishStreamConnection()
    {
        return () => RunSync(async () =>
        {
            logger.LogDebug("Establishing connection to space stream.");

            var streamId = await SpaceGrain.ConnectAsync();
            var provider = client.GetStreamProvider(StreamNames.PubSubProvider);
            var stream = provider.GetStream<SpaceTuple>(streamId, StreamNamespaces.TupleWrite);

            await stream.SubscribeAsync(this);

            logger.LogDebug("Space stream connection established.");
        });
    }

    private Action CloseClusterConnection()
    {
        return () => RunSync(async () =>
        {
            if (client.IsInitialized)
            {
                logger.LogDebug("Closing connection to cluster.");

                await client.Connect();

                logger.LogDebug("Cluster connection established.");
            }
        });
    }
}
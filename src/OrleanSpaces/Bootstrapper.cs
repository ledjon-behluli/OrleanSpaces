using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using OrleanSpaces.Routers;

namespace OrleanSpaces;

internal class Bootstrapper : BackgroundService, IGrainFactoryProvider
{
    private readonly ObserverRouter router;
    private readonly IClusterClient client;
    private readonly ILogger<Bootstrapper> logger;

    public IGrainFactory GrainFactory => client;

    public Bootstrapper(
        ObserverRouter agent,
        IClusterClient client,
        ILogger<Bootstrapper> logger)
    {
        this.router = agent ?? throw new ArgumentNullException(nameof(agent));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("{Service} started.", nameof(Bootstrapper));

        cancellationToken.Register(async () =>
        {
            try
            {
                if (client.IsInitialized)
                {
                    logger.LogDebug("Trying to stop {Service} gracefully.", nameof(IClusterClient));
                    await client.Close();
                }
            }
            catch { }
        });

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (!client.IsInitialized)
                {
                    await client.Connect();
                }

                await router.InitializeAsync(client);
                break;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        logger.LogDebug("{Service} stopped.", nameof(Bootstrapper));
    }
}

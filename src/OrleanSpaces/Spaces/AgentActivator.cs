using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleanSpaces.Spaces;

internal interface IGrainFactoryProvider
{
    IGrainFactory GrainFactory { get; }
}

internal class AgentActivator : BackgroundService, IGrainFactoryProvider
{
    private readonly SpaceAgent agent;
    private readonly IClusterClient client;
    private readonly ILogger<AgentActivator> logger;

    public IGrainFactory GrainFactory => client;

    public AgentActivator(
        SpaceAgent agent,
        IClusterClient client,
        ILogger<AgentActivator> logger)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Agent activator started.");

        cancellationToken.Register(async () => 
        { 
            try 
            {
                if (client.IsInitialized)
                {
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

                await agent.InitializeAsync(client);
                break;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to initialize space agent.");
                throw;
            }
        }

        logger.LogDebug("Agent activator stopped.");
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleanSpaces.Spaces;

internal class AgentActivator : BackgroundService
{
    private readonly SpaceAgent agent;
    private readonly IClusterClient client;
    private readonly ILogger<AgentActivator> logger;

    public AgentActivator(
        SpaceAgent agent,
        IClusterClient client,
        ILogger<AgentActivator> logger)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.client = client ?? throw new ArgumentNullException(nameof(agent));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Agent activator started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            if (client.IsInitialized)
            {
                try
                {
                    await agent.InitializeAsync(client);
                    break;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to initialize space agent.");
                    throw;
                }
            }

            await Task.Delay(100);
        }

        logger.LogDebug("Agent activator stopped.");
    }
}

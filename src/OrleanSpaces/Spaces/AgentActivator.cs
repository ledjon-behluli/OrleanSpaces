using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrleanSpaces.Spaces;

internal class AgentActivator : BackgroundService
{
    private readonly ILogger<AgentActivator> logger;
    private readonly SpaceAgent agent;

    public AgentActivator(
        SpaceAgent agent,
        ILogger<AgentActivator> logger)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogDebug("Initializing Space gent.");

            await agent.InitializeAsync();

            logger.LogDebug("Space agent initialized successfully.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to initialize space agent.");
            throw;
        }
    }
}

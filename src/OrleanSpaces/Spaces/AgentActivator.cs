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
            await agent.ActivateAsync();
            logger.LogInformation("Space agent activated successfully.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to activate space agent.");
            throw;
        }
    }
}

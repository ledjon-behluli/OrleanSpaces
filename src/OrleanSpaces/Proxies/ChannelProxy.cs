using Microsoft.Extensions.Logging;

namespace OrleanSpaces.Proxies;

internal class ChannelProxy : ISpaceChannelProxy
{
    private readonly SpaceAgent agent;
    private readonly ILogger<ChannelProxy> logger;

    public ChannelProxy(
        SpaceAgent agent,
        ILogger<ChannelProxy> logger)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<ISpaceChannel> OpenAsync(CancellationToken cancellationToken)
    {
        if (!agent.IsInitialized)
        {
            logger.LogDebug("Initializing space agent.");

            await agent.InitAsync();

            logger.LogDebug("Space agent initialized.");
        }

        return agent;
    }
}
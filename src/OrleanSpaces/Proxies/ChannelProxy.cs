using Microsoft.Extensions.Logging;
using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Proxies;

internal class ChannelProxy : ISpaceChannelProxy
{
    private readonly IClusterClient client;
    private readonly ILogger<ChannelProxy> logger;
    private readonly ICallbackRegistry callbackRegistry;
    private readonly IObserverRegistry observerRegistry;

    private SpaceAgent? agent;

    public ChannelProxy(
        IClusterClient client,
        ILogger<ChannelProxy> logger,
        ICallbackRegistry callbackRegistry,
        IObserverRegistry observerRegistry)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.callbackRegistry = callbackRegistry ?? throw new ArgumentNullException(nameof(callbackRegistry));
        this.observerRegistry = observerRegistry ?? throw new ArgumentNullException(nameof(observerRegistry));
    }

    public async ValueTask<ISpaceChannel> OpenAsync(CancellationToken cancellationToken)
    {
        if (agent is null)
        {
            agent = new SpaceAgent(logger, client, callbackRegistry, observerRegistry);
            await agent.InitAsync();
        }

        return agent;
    }
}
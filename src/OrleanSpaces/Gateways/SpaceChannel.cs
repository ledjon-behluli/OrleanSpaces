using Microsoft.Extensions.Logging;
using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Gateways;

internal class SpaceChannel : ISpaceChannel
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceChannel(
        ILogger<SpaceAgent> logger,
        IClusterClient client,
        CallbackRegistry callbackRegistry,
        ObserverRegistry observerRegistry)
    {
        agent = new(logger, client, callbackRegistry, observerRegistry);
    }

    public async Task<ISpaceAgent> GetAsync()
    {
        await semaphore.WaitAsync();

        try
        {
            if (!initialized)
            {
                await agent.InitializeAsync();
                initialized = true;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}
using Microsoft.Extensions.Logging;
using Orleans;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;

namespace OrleanSpaces;

internal partial class SpaceChannel : ISpaceChannel
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceChannel(
        ILogger<SpaceChannel> logger,
        IClusterClient client,
        EvaluationChannel evaluationChannel,
        CallbackChannel callbackChannel,
        ObserverChannel observerChannel,
        CallbackRegistry callbackRegistry,
        ObserverRegistry observerRegistry)
    {
        agent = new(logger, client, evaluationChannel, callbackChannel, observerChannel, callbackRegistry, observerRegistry);
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
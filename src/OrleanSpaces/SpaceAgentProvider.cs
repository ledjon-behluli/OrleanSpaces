using OrleanSpaces.Agents;

namespace OrleanSpaces;

public interface ISpaceAgentProvider
{
    /// <summary>
    /// Returns an agent that is used to interact with the tuple space.
    /// </summary>
    /// <remarks><i>Method is thread-safe.</i></remarks>
    ValueTask<ISpaceAgent> GetAsync();
}

internal sealed class SpaceAgentProvider : ISpaceAgentProvider
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceAgentProvider(SpaceAgent agent)
    {
        this.agent = agent;
    }

    public async ValueTask<ISpaceAgent> GetAsync()
    {
        if (initialized)
        {
            return agent;
        }

        await semaphore.WaitAsync();

        try
        {
            await agent.InitializeAsync();
            initialized = true;
        }
        finally
        {
            semaphore.Release();
        }

        return agent;
    }
}
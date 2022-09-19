namespace OrleanSpaces;

public interface ISpaceChannel
{
    /// <summary>
    /// Opens a channel between the client and the tuple space.
    /// </summary>
    /// <returns>An instance of <see cref="ISpaceAgent"/> that is used to interact with the tuple space.</returns>
    /// <remarks><i>Method is thread-safe.</i></remarks>
    Task<ISpaceAgent> OpenAsync();
}

internal sealed class SpaceChannel : ISpaceChannel
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceChannel(SpaceAgent agent)
    {
        this.agent = agent;
    }

    public async Task<ISpaceAgent> OpenAsync()
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
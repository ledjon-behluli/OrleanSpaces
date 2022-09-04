namespace OrleanSpaces;

internal class SpaceChannel : ISpaceChannel
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly SpaceAgent agent;
    private bool initialized;

    public SpaceChannel(SpaceAgent agent)
    {
        this.agent = agent;
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
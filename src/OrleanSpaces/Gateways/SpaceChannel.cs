namespace OrleanSpaces.Gateways;

internal class SpaceChannel : ISpaceChannel
{
    private readonly SpaceAgent agent;

    private bool initialized;
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public SpaceChannel(SpaceAgent agent)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
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
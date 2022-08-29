namespace OrleanSpaces.Bridges;

/// <summary>
/// Represents a one-way bridge between clients and <see cref="ISpaceChannel"/>.
/// </summary>
internal class SpaceChannelBridge : ISpaceChannelProvider
{
    private readonly SpaceGrainBridge bridge;

    private bool initialized;
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public SpaceChannelBridge(SpaceGrainBridge bridge)
    {
        this.bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
    }

    public async Task<ISpaceChannel> GetAsync()
    {
        await semaphore.WaitAsync();
        try
        {
            if (!initialized)
            {
                await bridge.InitAsync();
                initialized = true;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return bridge;
    }
}
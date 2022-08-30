namespace OrleanSpaces.Bridges;

/// <summary>
/// Represents a one-way bridge between clients and <see cref="ISpaceChannel"/>.
/// </summary>
internal class SpaceChannelBridge : ISpaceChannelProvider
{
    private readonly SpaceAgent bridge;

    private bool isConnected;
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public SpaceChannelBridge(SpaceAgent bridge)
    {
        this.bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
    }

    public async Task<ISpaceChannel> GetAsync()
    {
        await semaphore.WaitAsync();
        try
        {
            if (!isConnected)
            {
                await bridge.ConnectAsync();
                isConnected = true;
            }
        }
        finally
        {
            semaphore.Release();
        }

        return bridge;
    }
}
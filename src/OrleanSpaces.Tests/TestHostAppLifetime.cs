using Microsoft.Extensions.Hosting;

namespace OrleanSpaces.Tests;

public class TestHostAppLifetime : IHostApplicationLifetime
{
    private readonly CancellationTokenSource stoppedSource = new();

    public CancellationToken ApplicationStarted => CancellationToken.None;
    public CancellationToken ApplicationStopping => CancellationToken.None;
    public CancellationToken ApplicationStopped => stoppedSource.Token;

    public void StopApplication() => stoppedSource.Cancel();
}

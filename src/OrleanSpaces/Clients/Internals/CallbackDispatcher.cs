using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrleanSpaces.Clients.Internals;

internal class CallbackDispatcher : BackgroundService
{
    private readonly ILogger<CallbackDispatcher> logger;

    public CallbackDispatcher(ILogger<CallbackDispatcher> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Callback dispatcher started.");

        await foreach (CallbackBag bag in CallbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                await bag.DispatchAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogInformation("Callback dispatcher stopped.");
    }
}

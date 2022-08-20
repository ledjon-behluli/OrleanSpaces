using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace OrleanSpaces.Clients.Callbacks;

internal class CallbackDispatcher : BackgroundService
{
    private readonly ChannelReader<CallbackBag> channelReader;
    private readonly ILogger<CallbackDispatcher> logger;

    public CallbackDispatcher(
        CallbackChannel channel,
        ILogger<CallbackDispatcher> logger)
    {
        channelReader = (channel ?? throw new ArgumentNullException(nameof(channel))).Reader;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Callback dispatcher started.");

        await foreach (CallbackBag bag in channelReader.ReadAllAsync(cancellationToken))
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

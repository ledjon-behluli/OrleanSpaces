using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;
using System.Collections.Concurrent;

namespace OrleanSpaces.Observers;

internal class ObserverManager : BackgroundService, IObserverRegistry
{
    private readonly ILogger<ObserverManager> logger;
    private readonly ConcurrentDictionary<ISpaceObserver, Guid> observers = new();

    public ObserverManager(ILogger<ObserverManager> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Guid Register(ISpaceObserver observer)
    {
        if (!observers.TryGetValue(observer, out Guid id))
        {
            observers.TryAdd(observer, id);
        }

        return id;
    }

    public void Deregister(ISpaceObserver observer)
        => observers.TryRemove(observer, out _);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Observer manager started.");

        await foreach (SpaceTuple tuple in ObserverChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                var tasks = observers
                    .Select(pair => new Task(() => pair.Key.OnTupleAsync(tuple)))
                    .ToList();

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogInformation("Observer manager stopped.");
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Callbacks;

internal class CallbackManager : BackgroundService, ICallbackRegistry
{
    private readonly ILogger<CallbackManager> logger;
    private readonly ConcurrentDictionary<SpaceTemplate, List<Func<SpaceTuple, Task>>> callbacks = new();

    public CallbackManager(ILogger<CallbackManager> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (!callbacks.ContainsKey(template))
        {
            callbacks.TryAdd(template, new());
        }

        callbacks[template].Add(callback);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback manager started.");

        await foreach (var tuple in CallbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                //TODO: Check why this is no pulling...I think its because Blocking is not done as worker services
                await RunCallbacksAsync(tuple);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback manager stopped.");
    }

    private async Task RunCallbacksAsync(SpaceTuple tuple)
    {
        List<Func<SpaceTuple, Task>> matchingCallbacks = new();

        foreach (var pair in callbacks.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var callback in callbacks[pair.Key])
                {
                    matchingCallbacks.Add(callback);
                }

                callbacks.TryRemove(pair.Key, out _);
            }
        }

        await ParallelExecutor.WhenAll(matchingCallbacks, callback => callback(tuple));
    }
}
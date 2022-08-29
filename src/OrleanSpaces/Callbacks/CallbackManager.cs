using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;
using OrleanSpaces.Proxies;
using OrleanSpaces.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Callbacks;

internal class CallbackManager : BackgroundService, ICallbackRegistry
{
    private readonly SpaceAgent agent;
    private readonly ILogger<CallbackManager> logger;
    private readonly ConcurrentDictionary<SpaceTemplate, List<CallbackEntry>> entries = new();

    public CallbackManager(
        SpaceAgent agent,
        ILogger<CallbackManager> logger)
    {
        this.agent = agent ?? throw new ArgumentNullException(nameof(agent));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Register(SpaceTemplate template, CallbackEntry entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback manager started.");

        await foreach (var tuple in CallbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                await RunEntriesAsync(tuple);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback manager stopped.");
    }

    private async Task RunEntriesAsync(SpaceTuple tuple)
    {
        List<CallbackEntry> matchingEntries = new();

        foreach (var pair in entries.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var callback in entries[pair.Key])
                {
                    matchingEntries.Add(callback);
                }

                entries.TryRemove(pair.Key, out _);
            }
        }

        await ParallelExecutor.WhenAll(matchingEntries, async entry =>
        {
            await entry.Callback(tuple);
            if (entry.Destructive)
            {
                await agent.PopAsync(SpaceTemplate.Create(tuple));
            }
        });
    }
}
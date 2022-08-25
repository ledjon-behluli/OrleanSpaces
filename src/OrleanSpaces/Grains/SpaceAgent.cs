using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Grains;


[ImplicitStreamSubscription(Constants.StreamNamespace)]
internal class SpaceAgent : Grain, ICallbackRegistry, IAsyncObserver<SpaceTuple>
{
    private readonly ILogger<SpaceAgent> logger;
    private readonly ConcurrentDictionary<SpaceTemplate, List<Func<SpaceTuple, Task>>> templateCallbacks;

    public SpaceAgent(ILogger<SpaceAgent> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        templateCallbacks = new();
    }

    public override async Task OnActivateAsync()
    {
        await GetStreamProvider(Constants.StreamProviderName)
             .GetStream<SpaceTuple>(this.GetPrimaryKey(), Constants.StreamNamespace)
             .SubscribeAsync(this);

        await base.OnActivateAsync();
    }

    public void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (!templateCallbacks.ContainsKey(template))
        {
            templateCallbacks.TryAdd(template, new());
        }

        templateCallbacks[template].Add(callback);
    }

    public async Task OnNextAsync(SpaceTuple tuple, StreamSequenceToken token)
    {
        foreach (var pair in templateCallbacks.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var callback in templateCallbacks[pair.Key])
                {
                    await CallbackChannel.Writer.WriteAsync(new(tuple, callback));
                }

                templateCallbacks.TryRemove(pair.Key, out _);
            }
        }
    }

    public Task OnCompletedAsync() => Task.CompletedTask;

    public Task OnErrorAsync(Exception e)
    {
        logger.LogError(e, e.Message);
        return Task.CompletedTask;
    }
}
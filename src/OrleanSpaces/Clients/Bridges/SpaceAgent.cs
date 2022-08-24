using OrleanSpaces.Clients.Callbacks;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Clients.Bridges;

internal class SpaceAgent : ICallbackRegistry, ISpaceObserver
{
    private readonly ConcurrentDictionary<SpaceTemplate, List<Func<SpaceTuple, Task>>> templateCallbacks = new();

    public void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (!templateCallbacks.ContainsKey(template))
        {
            templateCallbacks.TryAdd(template, new());
        }

        templateCallbacks[template].Add(callback);
    }

    public async Task ReceiveAsync(SpaceTuple tuple)
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
}
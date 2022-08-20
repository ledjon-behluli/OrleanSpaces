using OrleanSpaces.Core;
using OrleanSpaces.Core.Internals;
using System.Collections.Concurrent;

namespace OrleanSpaces.Clients.Internals;

internal class SpaceAgent : ISpaceObserver, ICallbackRegistry
{
    private readonly ConcurrentDictionary<SpaceTemplate, List<Func<SpaceTuple, Task>>> templateCallbacks = new();

    public void Receive(SpaceTuple tuple)
    {
        foreach (var pair in templateCallbacks.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var callback in templateCallbacks[pair.Key])
                {
                    CallbackChannel.Writer.TryWrite(new(tuple, callback));
                }

                templateCallbacks.TryRemove(pair.Key, out _);
            }
        }
    }

    public void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (!templateCallbacks.ContainsKey(template))
        {
            templateCallbacks.TryAdd(template, new());
        }

        templateCallbacks[template].Add(callback);
    }
}
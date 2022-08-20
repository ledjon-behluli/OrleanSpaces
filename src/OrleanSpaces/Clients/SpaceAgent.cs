using OrleanSpaces.Clients.Callbacks;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace OrleanSpaces.Clients;

internal class SpaceAgent : ISpaceObserver, ICallbackBuffer
{
    private readonly ChannelWriter<CallbackBag> channelWriter;
    private readonly ConcurrentDictionary<SpaceTemplate, List<Func<SpaceTuple, Task>>> templateCallbacks = new();

    public SpaceAgent(CallbackChannel channel)
    {
        this.channelWriter = (channel ?? throw new ArgumentNullException(nameof(channel))).Writer;
    }

    public void Receive(SpaceTuple tuple)
    {
        foreach (var pair in templateCallbacks.Where(x => x.Key.Length == tuple.Length))
        {
            if (TupleMatcher.IsMatch(tuple, pair.Key))
            {
                foreach (var callback in templateCallbacks[pair.Key])
                {
                    channelWriter.TryWrite(new(tuple, callback));
                }

                templateCallbacks.TryRemove(pair.Key, out _);
            }
        }
    }

    public void Buffer(SpaceTemplate template, Func<SpaceTuple, Task> callback)
    {
        if (!templateCallbacks.ContainsKey(template))
        {
            templateCallbacks.TryAdd(template, new());
        }

        templateCallbacks[template].Add(callback);
    }
}
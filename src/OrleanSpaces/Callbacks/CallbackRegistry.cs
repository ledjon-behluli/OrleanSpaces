using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;
using System.Collections.Concurrent;

namespace OrleanSpaces.Callbacks;

internal class CallbackRegistry
{
    private readonly ConcurrentDictionary<SpaceTemplate, List<CallbackEntry>> entries = new();

    public void Add(SpaceTemplate template, CallbackEntry entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }

    public IList<CallbackEntry> Take(SpaceTuple tuple)
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

        return matchingEntries;
    }
}
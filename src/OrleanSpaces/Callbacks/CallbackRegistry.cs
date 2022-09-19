using OrleanSpaces.Primitives;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackRegistry
{
    private readonly ConcurrentDictionary<SpaceTemplate, List<CallbackEntry>> entries = new();

    public ReadOnlyDictionary<SpaceTemplate, ReadOnlyCollection<CallbackEntry>> Entries =>
      new(entries.ToDictionary(k => k.Key, v => v.Value.AsReadOnly()));

    public void Add(SpaceTemplate template, CallbackEntry entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }

    public IEnumerable<CallbackEntry> Take(SpaceTuple tuple)
    {
        foreach (var pair in entries.Where(x => x.Key.Length == tuple.Length))
        {
            if(pair.Key.Matches(tuple))
            {
                foreach (var entry in entries[pair.Key])
                {
                    yield return entry;
                }

                entries.TryRemove(pair.Key, out _);
            }
        }
    }
}
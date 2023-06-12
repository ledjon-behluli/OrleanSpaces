using OrleanSpaces.Tuples;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackRegistry
{
    private readonly ConcurrentDictionary<ISpaceElement, List<CallbackEntry>> entries = new();

    public ReadOnlyDictionary<ISpaceElement, ReadOnlyCollection<CallbackEntry>> Entries =>
        new(entries.ToDictionary(k => k.Key, v => v.Value.AsReadOnly()));

    public void Add(ISpaceElement template, CallbackEntry entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }

    public IEnumerable<CallbackEntry> Take(ISpaceElement tuple)
    {
        foreach (var pair in entries.Where(x => x.Key.Length == tuple.Length))
        {
            if (pair.Key.Matches(tuple))
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
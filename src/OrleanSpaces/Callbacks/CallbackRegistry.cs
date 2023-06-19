using OrleanSpaces.Tuples;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackRegistry<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly ConcurrentDictionary<TTemplate, List<CallbackEntry<TTuple>>> entries = new();

    public ReadOnlyDictionary<TTemplate, ReadOnlyCollection<CallbackEntry<TTuple>>> Entries =>
        new(entries.ToDictionary(k => k.Key, v => v.Value.AsReadOnly()));

    public void Add(TTemplate template, CallbackEntry<TTuple> entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }
    
    public IEnumerable<CallbackEntry<TTuple>> Take(TTuple tuple)
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
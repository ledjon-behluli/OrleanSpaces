using OrleanSpaces.Tuples;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;

namespace OrleanSpaces.Callbacks;

internal interface ICallbackRegistry<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    void Add(TTemplate template, CallbackEntry<TTuple> entry);
    IEnumerable<CallbackEntry<TTuple>> Take(TTuple tuple);
}

internal sealed class CallbackRegistry : ICallbackRegistry<SpaceTuple, SpaceTemplate>
{
    private readonly ConcurrentDictionary<SpaceTemplate, List<CallbackEntry<SpaceTuple>>> entries = new();

    public ReadOnlyDictionary<SpaceTemplate, ReadOnlyCollection<CallbackEntry<SpaceTuple>>> Entries =>
        new(entries.ToDictionary(k => k.Key, v => v.Value.AsReadOnly()));

    public void Add(SpaceTemplate template, CallbackEntry<SpaceTuple> entry)
    {
        if (!entries.ContainsKey(template))
        {
            entries.TryAdd(template, new());
        }

        entries[template].Add(entry);
    }

    public IEnumerable<CallbackEntry<SpaceTuple>> Take(SpaceTuple tuple)
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

internal sealed class CallbackRegistry<T, TTuple, TTemplate> : ICallbackRegistry<TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
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
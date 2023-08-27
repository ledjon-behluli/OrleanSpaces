using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Collections;

internal sealed class ReadOptimizedCollection : ITupleCollection
{
    private readonly ConcurrentDictionary<int, ImmutableArray<SpaceTuple>> dict = new();

    public int Count => dict.Values.Sum(x => x.Length);

    public void Add(SpaceTuple tuple)
    {
        int index = tuple.Length - 1;
        dict[index] = dict.TryGetValue(index, out ImmutableArray<SpaceTuple> tuples) ?
            tuples.Add(tuple) : ImmutableArray.Create(tuple);
    }

    public void Remove(SpaceTuple tuple)
    {
        int index = tuple.Length - 1;
        if (dict.TryRemove(index, out ImmutableArray<SpaceTuple> tuples))
        {
            dict[index] = tuples.Remove(tuple);
        }
    }

    public void Clear() => dict.Clear();

    public SpaceTuple Find(SpaceTemplate template)
    {
        int index = template.Length - 1;
        if (dict.ContainsKey(index))
        {
            foreach (SpaceTuple tuple in dict[index])
            {
                if (template.Matches(tuple))
                {
                    return tuple;
                }
            }
        }

        return default;
    }

    public IEnumerable<SpaceTuple> FindAll(SpaceTemplate template)
    {
        List<SpaceTuple> tuples = new();
        int index = template.Length - 1;

        if (dict.ContainsKey(index))
        {
            foreach (SpaceTuple tuple in dict[index])
            {
                if (template.Matches(tuple))
                {
                    tuples.Add(tuple);
                }
            }
        }

        return tuples;
    }

    public IEnumerator<SpaceTuple> GetEnumerator()
    {
        foreach (var kvp in dict)
        {
            foreach (SpaceTuple tuple in kvp.Value)
            {
                yield return tuple;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class ReadOptimizedCollection<T, TTuple, TTemplate> : ITupleCollection<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly ConcurrentDictionary<int, ImmutableArray<TTuple>> dict = new();

    public int Count => dict.Values.Sum(x => x.Length);

    public void Add(TTuple tuple)
    {
        int index = tuple.Length - 1;
        dict[index] = dict.TryGetValue(index, out ImmutableArray<TTuple> tuples) ?
            tuples.Add(tuple) : ImmutableArray.Create(tuple);
    }

    public void Remove(TTuple tuple)
    {
        int index = tuple.Length - 1;
        if (dict.TryRemove(index, out ImmutableArray<TTuple> tuples))
        {
            dict[index] = tuples.Remove(tuple);
        }
    }

    public void Clear() => dict.Clear();

    public TTuple Find(TTemplate template)
    {
        int index = template.Length - 1;
        if (dict.ContainsKey(index))
        {
            foreach (TTuple tuple in dict[index])
            {
                if (template.Matches(tuple))
                {
                    return tuple;
                }
            }
        }

        return default;
    }

    public IEnumerable<TTuple> FindAll(TTemplate template)
    {
        List<TTuple> tuples = new();
        int index = template.Length - 1;

        if (dict.ContainsKey(index))
        {
            foreach (TTuple tuple in dict[index])
            {
                if (template.Matches(tuple))
                {
                    tuples.Add(tuple);
                }
            }
        }

        return tuples;
    }

    public IEnumerator<TTuple> GetEnumerator()
    {
        foreach (var kvp in dict)
        {
            foreach (TTuple tuple in kvp.Value)
            {
                yield return tuple;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal record struct TupleAddressPair<T>(T Tuple, Guid StoreId) where T : ISpaceTuple;

internal sealed class TupleCollection : IEnumerable<SpaceTuple>
{
    private readonly ConcurrentDictionary<int, ImmutableArray<TupleAddressPair<SpaceTuple>>> dict = new();

    public int Count => dict.Values.Sum(x => x.Length);

    public void Add(TupleAddressPair<SpaceTuple> pair)
    {
        int index = pair.Tuple.Length - 1;
        dict[index] = dict.TryGetValue(index, out ImmutableArray<TupleAddressPair<SpaceTuple>> pairs) ?
            pairs.Add(pair) : ImmutableArray.Create(pair);
    }

    public void Remove(TupleAddressPair<SpaceTuple> pair)
    {
        int index = pair.Tuple.Length - 1;
        if (dict.TryRemove(index, out ImmutableArray<TupleAddressPair<SpaceTuple>> pairs))
        {
            dict[index] = pairs.Remove(pair);
        }
    }

    public void Clear() => dict.Clear();

    public TupleAddressPair<SpaceTuple> FindPair(SpaceTemplate template)
    {
        int index = template.Length - 1;
        if (dict.ContainsKey(index))
        {
            foreach (var pair in dict[index])
            {
                if (template.Matches(pair.Tuple))
                {
                    return pair;
                }
            }
        }

        return default;
    }

    public IEnumerable<SpaceTuple> FindAllTuples(SpaceTemplate template)
    {
        List<SpaceTuple> tuples = new();
        int index = template.Length - 1;

        if (dict.ContainsKey(index))
        {
            foreach (var pair in dict[index])
            {
                if (template.Matches(pair.Tuple))
                {
                    tuples.Add(pair.Tuple);
                }
            }
        }

        return tuples;
    }

    public IEnumerator<SpaceTuple> GetEnumerator()
    {
        foreach (var kvp in dict)
        {
            foreach (var pair in kvp.Value)
            {
                yield return pair.Tuple;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class TupleCollection<T, TTuple, TTemplate> : IEnumerable<TTuple>
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

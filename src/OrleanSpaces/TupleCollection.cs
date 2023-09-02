using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal sealed class TupleCollection : IEnumerable<SpaceTuple>
{
    private readonly ConcurrentDictionary<int, ImmutableArray<TupleAddress<SpaceTuple>>> dict = new();

    public int Count => dict.Values.Sum(x => x.Length);

    public void Add(TupleAddress<SpaceTuple> item)
    {
        int index = item.Tuple.Length - 1;
        dict[index] = dict.TryGetValue(index, out ImmutableArray<TupleAddress<SpaceTuple>> items) ?
            items.Add(item) : ImmutableArray.Create(item);
    }

    public void Remove(TupleAddress<SpaceTuple> item)
    {
        int index = item.Tuple.Length - 1;
        if (dict.TryRemove(index, out ImmutableArray<TupleAddress<SpaceTuple>> items))
        {
            dict[index] = items.Remove(item);
        }
    }

    public void Clear() => dict.Clear();

    public TupleAddress<SpaceTuple> Find(SpaceTemplate template)
    {
        int index = template.Length - 1;
        if (dict.ContainsKey(index))
        {
            foreach (var item in dict[index])
            {
                if (template.Matches(item.Tuple))
                {
                    return item;
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
            foreach (var item in dict[index])
            {
                if (template.Matches(item.Tuple))
                {
                    tuples.Add(item.Tuple);
                }
            }
        }

        return tuples;
    }

    public IEnumerator<SpaceTuple> GetEnumerator()
    {
        foreach (var kvp in dict)
        {
            foreach (var item in kvp.Value)
            {
                yield return item.Tuple;
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
    private readonly ConcurrentDictionary<int, ImmutableArray<TupleAddress<TTuple>>> dict = new();

    public int Count => dict.Values.Sum(x => x.Length);

    public void Add(TupleAddress<TTuple> item)
    {
        int index = item.Tuple.Length - 1;
        dict[index] = dict.TryGetValue(index, out ImmutableArray<TupleAddress<TTuple>> tuples) ?
            tuples.Add(item) : ImmutableArray.Create(item);
    }

    public void Remove(TupleAddress<TTuple> item)
    {
        int index = item.Tuple.Length - 1;
        if (dict.TryRemove(index, out ImmutableArray<TupleAddress<TTuple>> tuples))
        {
            dict[index] = tuples.Remove(item);
        }
    }

    public void Clear() => dict.Clear();

    public TupleAddress<TTuple> Find(TTemplate template)
    {
        int index = template.Length - 1;
        if (dict.ContainsKey(index))
        {
            foreach (var item in dict[index])
            {
                if (template.Matches(item.Tuple))
                {
                    return item;
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
            foreach (var item in dict[index])
            {
                if (template.Matches(item.Tuple))
                {
                    tuples.Add(item.Tuple);
                }
            }
        }

        return tuples;
    }

    public IEnumerator<TTuple> GetEnumerator()
    {
        foreach (var kvp in dict)
        {
            foreach (var item in kvp.Value)
            {
                yield return item.Tuple;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

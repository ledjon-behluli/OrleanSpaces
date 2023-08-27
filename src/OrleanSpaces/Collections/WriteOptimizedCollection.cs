using System.Collections;
using System.Collections.Immutable;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Collections;

internal sealed class WriteOptimizedCollection : ITupleCollection<SpaceTuple, SpaceTemplate>
{
    private ImmutableArray<SpaceTuple> array = ImmutableArray<SpaceTuple>.Empty;

    public int Count => array.Length;

    public void Add(SpaceTuple tuple) => array = array.Add(tuple);
    public void Remove(SpaceTuple tuple) => array = array.Remove(tuple);
    public void Clear() => array = ImmutableArray<SpaceTuple>.Empty;

    public SpaceTuple Find(SpaceTemplate template)
    {
        foreach (SpaceTuple tuple in array)
        {
            if (template.Matches(tuple))
            {
                return tuple;
            }
        }

        return default;
    }

    public IEnumerable<SpaceTuple> FindAll(SpaceTemplate template)
    {
        List<SpaceTuple> result = new();

        foreach (SpaceTuple tuple in array)
        {
            if (template.Matches(tuple))
            {
                result.Add(tuple);
            }
        }

        return result;
    }

    public IEnumerator<SpaceTuple> GetEnumerator()
    {
        foreach (SpaceTuple tuple in array)
        {
            yield return tuple;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class WriteOptimizedCollection<T, TTuple, TTemplate> : ITupleCollection<TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private ImmutableArray<TTuple> array = ImmutableArray<TTuple>.Empty;

    public int Count => array.Length;

    public void Add(TTuple tuple) => array = array.Add(tuple);
    public void Remove(TTuple tuple) => array = array.Remove(tuple);
    public void Clear() => array = ImmutableArray<TTuple>.Empty;

    public TTuple Find(TTemplate template)
    {
        foreach (TTuple tuple in array)
        {
            if (template.Matches(tuple))
            {
                return tuple;
            }
        }

        return default;
    }

    public IEnumerable<TTuple> FindAll(TTemplate template)
    {
        List<TTuple> result = new();

        foreach (TTuple tuple in array)
        {
            if (template.Matches(tuple))
            {
                result.Add(tuple);
            }
        }

        return result;
    }

    public IEnumerator<TTuple> GetEnumerator()
    {
        foreach (TTuple tuple in array)
        {
            yield return tuple;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

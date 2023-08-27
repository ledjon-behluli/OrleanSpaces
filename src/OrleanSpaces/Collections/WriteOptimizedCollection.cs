using System.Collections;
using System.Collections.Immutable;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Collections;

internal sealed class WriteOptimizedCollection : ITupleCollection
{
    private readonly ImmutableArray<SpaceTuple> tuples = ImmutableArray<SpaceTuple>.Empty;

    public int Count => throw new NotImplementedException();

    public void Add(SpaceTuple tuple) => tuples.Add(tuple);
    public void Remove(SpaceTuple tuple) => tuples.Remove(tuple);
    public void Clear() => tuples.Clear();

    public SpaceTuple Find(SpaceTemplate template)
    {
        foreach (var tuple in tuples)
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
        List<SpaceTuple> tuples = new();

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                tuples.Add(tuple);
            }
        }

        return tuples;
    }

    public IEnumerator<SpaceTuple> GetEnumerator()
    {
        foreach (var tuple in tuples)
        {
            yield return tuple;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class WriteOptimizedCollection<T, TTuple, TTemplate> : ITupleCollection<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly ImmutableArray<TTuple> tuples = ImmutableArray<TTuple>.Empty;

    public int Count => throw new NotImplementedException();

    public void Add(TTuple tuple) => tuples.Add(tuple);
    public void Remove(TTuple tuple) => tuples.Remove(tuple);
    public void Clear() => tuples.Clear();

    public TTuple Find(TTemplate template)
    {
        foreach (var tuple in tuples)
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
        List<TTuple> tuples = new();

        foreach (var tuple in tuples)
        {
            if (template.Matches(tuple))
            {
                tuples.Add(tuple);
            }
        }

        return tuples;
    }

    public IEnumerator<TTuple> GetEnumerator()
    {
        foreach (var tuple in tuples)
        {
            yield return tuple;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

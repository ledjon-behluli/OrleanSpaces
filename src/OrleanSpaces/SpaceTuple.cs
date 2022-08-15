using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

public struct SpaceTuple : ITuple
{
    private readonly object[] _items;

    public int Length => _items.Length;
    public object this[int index] => _items[index];

    private SpaceTuple(params object[] items)
    {
        if (items.Length == 0)
        {
            throw new OrleanSpacesException($"At least one object must be present to construct a {nameof(SpaceTuple)}.");
        }

        if (items.Any(x => 
            x is SpaceUnit ||
            x is Type))
        {
            throw new OrleanSpacesException($"{nameof(Type)} and {nameof(SpaceUnit)} objects are not valid {nameof(SpaceTuple)} elements.");
        }

        _items = items;
    }

    public static SpaceTuple Create(object item) => new(item);

    public static SpaceTuple Create(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var items = new object[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            items[i] = tuple[i];
        }

        return new(items);
    }
}

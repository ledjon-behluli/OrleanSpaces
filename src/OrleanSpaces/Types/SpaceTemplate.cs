using System.Runtime.CompilerServices;
using OrleanSpaces.Exceptions;

namespace OrleanSpaces.Types;

[Serializable]
public struct SpaceTemplate : ITuple
{
    private readonly object[] _fields;

    public int Length => _fields.Length;
    public object this[int index] => _fields[index];

    private SpaceTemplate(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new TupleFieldLengthException(nameof(SpaceTemplate));
        }

        _fields = fields;
    }

    public static SpaceTemplate Create(object field) => new(field);

    public static SpaceTemplate Create(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var fields = new object[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            fields[i] = tuple[i];
        }

        return new(fields);
    }
}

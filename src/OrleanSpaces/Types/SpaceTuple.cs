using OrleanSpaces.Exceptions;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Types;

[Serializable]
public struct SpaceTuple : ITuple
{
    private readonly object[] _fields;

    public int Length => _fields.Length;
    public object this[int index] => _fields[index];

    private SpaceTuple(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new TupleFieldLengthException(nameof(SpaceTuple));
        }

        if (fields.Any(x => x is NullTuple || x is Type))
        {
            throw new TupleFieldException();
        }

        _fields = fields;
    }

    public static SpaceTuple Create(object field) => new(field);

    public static SpaceTuple Create(ITuple tuple)
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

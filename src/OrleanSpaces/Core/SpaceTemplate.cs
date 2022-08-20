using System.Runtime.CompilerServices;

namespace OrleanSpaces.Core;

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
            throw new ArgumentException($"Construction of '{nameof(SpaceTemplate)}' without any fields is not allowed.");
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

using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTuple : ISpaceElement, ITuple, IEquatable<SpaceTuple>
{
    private readonly ValueType[] _fields;

    public int Length => _fields?.Length ?? 0;
    public object this[int index] => _fields[index];

    public bool IsEmpty => _fields == null || _fields.Length == 0;

    public SpaceTuple() : this(new ValueType[0]) { }

    private SpaceTuple(params ValueType[] fields)
    {
        if (fields.Any(x => x is UnitField))
        {
            throw new ArgumentException("UnitField.Null is not a valid SpaceTuple field.");
        }

        _fields = fields ?? new ValueType[0];
    }

    public static SpaceTuple Create(ValueType field)
    {
        if (field is null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        return new(field);
    }

    public static SpaceTuple CreateFromTuple(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var fields = new ValueType[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            var field = tuple[i];
            if (field is not ValueType)
            {
                throw new ArgumentException("Type declarations");
            }

            fields[i] = (ValueType)tuple[i];
        }

        return new(fields);
    }

    public static SpaceTuple Create(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var fields = new ValueType[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            var field = tuple[i];
            if (field is not ValueType)
            {
                throw new ArgumentException("Type declarations");
            }

            fields[i] = (ValueType)tuple[i];
        }

        return new(fields);
    }

    public static bool operator ==(SpaceTuple first, SpaceTuple second) => first.Equals(second);
    public static bool operator !=(SpaceTuple first, SpaceTuple second) => !(first == second);

    public override bool Equals(object obj) =>
        obj is SpaceTuple tuple && Equals(tuple);

    public bool Equals(SpaceTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (!this[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => HashCode.Combine(_fields, Length);

    public override string ToString() => $"<{string.Join(", ", (object[])_fields)}>";
}

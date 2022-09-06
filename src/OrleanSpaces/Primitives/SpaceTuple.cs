using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTuple : ISpaceElement, ITuple, IEquatable<SpaceTuple>
{
    private readonly object[] _fields;

    public int Length => _fields?.Length ?? 0;
    public object this[int index] => _fields[index];

    public bool IsEmpty => _fields == null || _fields.Length == 0;

    public SpaceTuple() : this(new object[0]) { }

    private SpaceTuple(params object[] fields)
    {
        if (fields.Any(x => x is null || x is UnitField))
        {
            throw new ArgumentException($"Nulls are not valid '{nameof(SpaceTuple)}' fields");
        }

        _fields = fields ?? new object[0];
    }

    public static SpaceTuple Create(ValueType value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value is ITuple tuple)
        {
            var fields = new object[tuple.Length];
            for (int i = 0; i < tuple.Length; i++)
            {
                if (tuple[i] is not ValueType && 
                    tuple[i] is not string)
                {
                    throw new ArgumentException($"Reference types are not valid '{nameof(SpaceTuple)}' fields");
                }

                fields[i] = tuple[i];
            }

            return new(fields);
        }

        return new(value);
    }

    public static SpaceTuple Create(string field)
    {
        if (string.IsNullOrEmpty(field))
        {
            throw new ArgumentNullException(nameof(field));
        }

        return new(field);
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

    public override string ToString() => $"<{string.Join(", ", _fields)}>";
}

using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTuple : ITuple, IEquatable<SpaceTuple>
{
    private readonly object[] _fields;

    public int Length => _fields.Length;
    public object this[int index] => _fields[index];

    public bool IsEmpty => _fields.Length == 0;

    public SpaceTuple() : this(new object[0]) { }

    private SpaceTuple(params object[] fields)
    {
        if (fields.Any(x => x is UnitField || x is Type))
        {
            throw new ArgumentException($"Type declarations and '{nameof(UnitField)}' are not valid '{nameof(SpaceTuple)}' fields.");
        }

        _fields = fields ?? new object[0];
    }

    public static SpaceTuple Create(object field)
    {
        if (field is null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        return new(field);
    }

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

    public static bool operator ==(SpaceTuple first, SpaceTuple second) => first.Equals(second);
    public static bool operator !=(SpaceTuple first, SpaceTuple second) => !(first == second);

    public override bool Equals(object obj) =>
        obj is SpaceTuple overrides && Equals(overrides);

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

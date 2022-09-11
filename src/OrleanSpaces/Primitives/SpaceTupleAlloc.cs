using System.Buffers;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceTupleAlloc : ISpaceTuple, IEquatable<SpaceTupleAlloc>
{
    private readonly object[] fields;
    public ref readonly object this[int index] => ref fields[index];

    public int Length => fields.Length;
    public bool IsEmpty => fields.Length == 0;

    public SpaceTupleAlloc() : this(null) { }

    private SpaceTupleAlloc(params object[]? fields)
    {
        this.fields = fields ?? Array.Empty<object>();
    }

    public static SpaceTupleAlloc Create(ITuple tuple)
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

    public static SpaceTupleAlloc CreatePool(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var pool = ArrayPool<object>.Shared;
        var fields = pool.Rent(tuple.Length);

        try
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                if (tuple[i] is not ValueType &&
                    tuple[i] is not string)
                {
                    throw new ArgumentException($"Reference types are not valid '{nameof(SpaceTupleAlloc)}' fields");
                }

                if (tuple[i] is SpaceUnit)
                {
                    throw new ArgumentException($"'{nameof(SpaceUnit.Null)}' is not a valid '{nameof(SpaceTupleAlloc)}' field");
                }

                fields[i] = tuple[i];
            }

            return new(fields);
        }
        finally
        {
            pool.Return(fields);
        }
    }

    public static SpaceTupleAlloc CreateSingle(ValueType value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new(value);
    }

    public static SpaceTupleAlloc CreateSingle(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new(value);
    }

    public static bool operator ==(SpaceTupleAlloc first, SpaceTupleAlloc second) => first.Equals(second);
    public static bool operator !=(SpaceTupleAlloc first, SpaceTupleAlloc second) => !(first == second);

    public override bool Equals(object obj) =>
        obj is SpaceTupleAlloc tuple && Equals(tuple);

    public bool Equals(SpaceTupleAlloc other)
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

    public override int GetHashCode() => HashCode.Combine(fields, Length);

    public override string ToString() => $"<{string.Join(", ", fields)}>";
}

using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct EnumTuple<T> : ISpaceTuple<T> , IEquatable<EnumTuple<T>>, IComparable<EnumTuple<T>>
    where T : unmanaged, Enum
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;  // since the underlying type is an 'int', we will use the value.

    private readonly T[] fields;

    public ref readonly T this[int index] => ref fields[index];
    public int Length => fields.Length;

    public EnumTuple() : this(Array.Empty<T>()) { }
    public EnumTuple(params T[] fields) => this.fields = fields;

    public static bool operator ==(EnumTuple<T> left, EnumTuple<T> right) => left.Equals(right);
    public static bool operator !=(EnumTuple<T> left, EnumTuple<T> right) => !(left == right);

    public override bool Equals(object? obj) => obj is EnumTuple<T> tuple && Equals(tuple);
    public bool Equals(EnumTuple<T> other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Length == 1)
        {
            return this[0].Equals(other[0]);
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
        }

        return Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => AreParallelEquals<byte>(in this, in other),
            TypeCode.SByte => AreParallelEquals<sbyte>(in this, in other),
            TypeCode.Int16 => AreParallelEquals<short>(in this, in other),
            TypeCode.UInt16 => AreParallelEquals<ushort>(in this, in other),
            TypeCode.Int32 => AreParallelEquals<int>(in this, in other),
            TypeCode.UInt32 => AreParallelEquals<uint>(in this, in other),
            TypeCode.Int64 => AreParallelEquals<long>(in this, in other),
            TypeCode.UInt64 => AreParallelEquals<ulong>(in this, in other),
            _ => throw new NotSupportedException()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool AreParallelEquals<TOut>(in EnumTuple<T> left, in EnumTuple<T> right)
        where TOut : unmanaged, INumber<TOut>
    {
        NumericMarshaller<T, TOut> marshaller = new(left.fields.AsSpan(), right.fields.AsSpan());
        return marshaller.Left.ParallelEquals(marshaller.Right);
    }

    public int CompareTo(EnumTuple<T> other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan()
    {
        int tupleLength = Length;

        SFEnum[] sfEnums = new SFEnum[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfEnums[i] = new(this[i]);
        }

        return new SFEnumTuple(sfEnums).AsSpan(MaxFieldCharLength);
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator() => new ReadOnlySpan<T>(fields).GetEnumerator();

    readonly record struct SFEnumTuple(params SFEnum[] Values) : ISpaceTuple<SFEnum>
    {
        public ref readonly SFEnum this[int index] => ref Values[index];
        public int Length => Values.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
    }

    readonly record struct SFEnum(T Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            charsWritten = 0;

            return Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => Helpers.CastAs<T, byte>(Value).TryFormat(destination, out charsWritten),
                TypeCode.SByte => Helpers.CastAs<T, sbyte>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int16 => Helpers.CastAs<T, short>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt16 => Helpers.CastAs<T, ushort>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int32 => Helpers.CastAs<T, int>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt32 => Helpers.CastAs<T, uint>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int64 => Helpers.CastAs<T, long>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt64 => Helpers.CastAs<T, ulong>(Value).TryFormat(destination, out charsWritten),
                _ => false
            };
        }
    }
}

[Immutable]
public readonly struct SpaceEnum<T>
     where T : unmanaged, Enum
{
    public readonly T Value;

    public SpaceEnum(T value) => Value = value;

    public static implicit operator SpaceEnum<T>(T value) => new(value);
    public static implicit operator T(SpaceEnum<T> value) => value.Value;
}

[Immutable]
public readonly struct EnumTemplate<T> : ISpaceTemplate<EnumTuple<T>>
    where T : unmanaged, Enum
{
    private readonly SpaceEnum<T>[] fields;

    public EnumTemplate([AllowNull] params SpaceEnum<T>[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceEnum<T>[1] { new SpaceUnit() } : fields;
}

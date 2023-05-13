using Orleans.Concurrency;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct EnumTuple<T> : ISpaceTuple<T, EnumTuple<T>> 
    where T : unmanaged, Enum
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-2147483648</example>
    internal const int MaxFieldCharLength = 11;  // since the underlying type is an 'int', we will use the value.

    private readonly T[] fields;
    private readonly TypeCode typeCode;

    public ref readonly T this[int index] => ref fields[index];
    public int Length => fields.Length;

    public EnumTuple() : this(Array.Empty<T>()) { }
    public EnumTuple(params T[] fields)
    {
        this.fields = fields;
        this.typeCode = Type.GetTypeCode(typeof(T));
    }

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
            return this[0] == other[0];
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
        }

        switch (typeCode)
        {
            case TypeCode.Byte: return AreParallelEquals<byte>(in this, in other);
            case TypeCode.SByte: return AreParallelEquals<sbyte>(in this, in other);
            case TypeCode.Int16: return AreParallelEquals<short>(in this, in other);
            case TypeCode.UInt16: return AreParallelEquals<ushort>(in this, in other);
            case TypeCode.Int32: return AreParallelEquals<int>(in this, in other);
            case TypeCode.UInt32: return AreParallelEquals<uint>(in this, in other);
            case TypeCode.Int64: return AreParallelEquals<long>(in this, in other);
            case TypeCode.UInt64: return AreParallelEquals<ulong>(in this, in other);
        }
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
            sfEnums[i] = new(Helpers.CastAs<T, int>(in this[i]));
        }

        return new SFEnumTuple(sfEnums).AsSpan(MaxFieldCharLength);
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator() => new ReadOnlySpan<T>(fields).GetEnumerator();

    readonly record struct SFEnumTuple(params SFEnum[] Values) : ISpaceTuple<SFEnum, SFEnumTuple>
    {
        public ref readonly SFEnum this[int index] => ref Values[index];
        public int Length => Values.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;

        public int CompareTo(SFEnumTuple other) => Length.CompareTo(other.Length);
    }

    readonly record struct SFEnum(int Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => Value.TryFormat(destination, out charsWritten);
    }
}
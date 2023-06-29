using Newtonsoft.Json;
using OrleanSpaces;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct EnumTuple<T> : ISpaceTuple<T> , IEquatable<EnumTuple<T>>
    where T : unmanaged, Enum
{
    [Id(0), JsonProperty] private readonly T[] fields;

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

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<char> AsSpan()
    {
        int tupleLength = Length;

        SFEnum[] sfEnums = new SFEnum[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfEnums[i] = new(this[i]);
        }

        int maxFieldCharLength = Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => Constants.MaxFieldCharLength_Byte,
            TypeCode.SByte => Constants.MaxFieldCharLength_SByte,
            TypeCode.Int16 => Constants.MaxFieldCharLength_Short,
            TypeCode.UInt16 => Constants.MaxFieldCharLength_UShort,
            TypeCode.Int32 => Constants.MaxFieldCharLength_Int,
            TypeCode.UInt32 => Constants.MaxFieldCharLength_UInt,
            TypeCode.Int64 => Constants.MaxFieldCharLength_Long,
            TypeCode.UInt64 => Constants.MaxFieldCharLength_ULong,
            _ => throw new NotSupportedException()
        };

        return new SFEnumTuple(sfEnums).AsSpan(maxFieldCharLength);
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator() => new ReadOnlySpan<T>(fields).GetEnumerator();

    readonly record struct SFEnumTuple(params SFEnum[] Fields) : ISpaceTuple<SFEnum>
    {
        public ref readonly SFEnum this[int index] => ref Fields[index];
        public int Length => Fields.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;

        public ReadOnlySpan<SFEnum>.Enumerator GetEnumerator() => new ReadOnlySpan<SFEnum>(Fields).GetEnumerator();
    }

    readonly record struct SFEnum(T Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) 
            => Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => TupleHelpers.CastAs<T, byte>(Value).TryFormat(destination, out charsWritten),
                TypeCode.SByte => TupleHelpers.CastAs<T, sbyte>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int16 => TupleHelpers.CastAs<T, short>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt16 => TupleHelpers.CastAs<T, ushort>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int32 => TupleHelpers.CastAs<T, int>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt32 => TupleHelpers.CastAs<T, uint>(Value).TryFormat(destination, out charsWritten),
                TypeCode.Int64 => TupleHelpers.CastAs<T, long>(Value).TryFormat(destination, out charsWritten),
                TypeCode.UInt64 => TupleHelpers.CastAs<T, ulong>(Value).TryFormat(destination, out charsWritten),
                _ => throw new NotSupportedException()
            };
    }
}

public readonly record struct EnumTemplate<T> : ISpaceTemplate<T>
    where T : unmanaged, Enum
{
    private readonly T?[] fields;

    public ref readonly T? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public EnumTemplate([AllowNull] params T?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new T?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<T> ISpaceTemplate<T>.Create(T[] fields) => new EnumTuple<T>(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<T?>.Enumerator GetEnumerator() => new ReadOnlySpan<T?>(fields).GetEnumerator();
}

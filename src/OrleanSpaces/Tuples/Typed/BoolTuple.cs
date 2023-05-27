﻿using Orleans.Concurrency;
using OrleanSpaces.Tuples;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct BoolTuple : ISpaceTuple<bool>, IEquatable<BoolTuple>, IComparable<BoolTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>False</example>
    internal const int MaxFieldCharLength = 5;

    private readonly bool[] fields;

    public ref readonly bool this[int index] => ref fields[index];
    public int Length => fields.Length;

    public BoolTuple() : this(Array.Empty<bool>()) { }
    public BoolTuple(params bool[] fields) => this.fields = fields;

    public static bool operator ==(BoolTuple left, BoolTuple right) => left.Equals(right);
    public static bool operator !=(BoolTuple left, BoolTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is BoolTuple tuple && Equals(tuple);

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(BoolTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<bool>.Enumerator GetEnumerator() => new ReadOnlySpan<bool>(fields).GetEnumerator();

    public ReadOnlySpan<char> AsSpan()
    {
        // Since `bool` does not implement `ISpanFormattable` (see: https://github.com/dotnet/runtime/issues/67388),
        // we cant use `Helpers.AsSpan`, and are forced to wrap it in a struct that implements it.

        int tupleLength = Length;

        SFBool[] sfBools = new SFBool[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfBools[i] = new(this[i]);
        }

        return new SFBoolTuple(sfBools).AsSpan(MaxFieldCharLength);
    }

    readonly record struct SFBoolTuple(params SFBool[] Values) : ISpaceTuple<SFBool>
    {
        public ref readonly SFBool this[int index] => ref Values[index];
        public int Length => Values.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
    }

    readonly record struct SFBool(bool Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => Value.TryFormat(destination, out charsWritten);
    }
}

[Immutable]
public readonly struct SpaceBool
{
    public readonly bool Value;

    internal static readonly SpaceBool Default = new();

    public SpaceBool(bool value) => Value = value;

    public static implicit operator SpaceBool(bool value) => new(value);
    public static implicit operator bool(SpaceBool value) => value.Value;
}

[Immutable]
public readonly struct BoolTemplate : ISpaceTemplate<BoolTuple>
{
    private readonly SpaceBool[] fields;

    public BoolTemplate([AllowNull] params SpaceBool[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceBool[1] { new SpaceUnit() } : fields;
}

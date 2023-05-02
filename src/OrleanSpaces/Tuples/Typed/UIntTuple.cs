﻿using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UIntTuple : INumericValueTuple<uint, UIntTuple>, ISpanFormattable
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>4294967295</example>
    internal const int MaxFieldCharLength = 10;

    private readonly uint[] fields;

    public ref readonly uint this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<uint> INumericValueTuple<uint, UIntTuple>.Fields => fields.AsSpan();

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(params uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => this.TryFormat(MaxFieldCharLength, destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => new ReadOnlySpan<uint>(fields).GetEnumerator();
}

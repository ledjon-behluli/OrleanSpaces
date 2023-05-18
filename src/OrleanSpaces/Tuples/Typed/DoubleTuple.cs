﻿using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double>, IEquatable<DoubleTuple>, IComparable<DoubleTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-1.7976931348623157E+308</example>
    internal const int MaxFieldCharLength = 24;

    private readonly double[] fields;

    public ref readonly double this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<double> INumericTuple<double>.Fields => fields.AsSpan();

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(params double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) 
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<double>.Enumerator GetEnumerator() => new ReadOnlySpan<double>(fields).GetEnumerator();
}
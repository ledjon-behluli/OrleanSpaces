﻿using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed.Numerics;

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double, DoubleTuple>
{
    private readonly double[] fields;

    public double this[int index] => fields[index];
    public int Length => fields.Length;

    Span<double> INumericTuple<double, DoubleTuple>.Data => fields.AsSpan();

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) => this.ParallelEquals(other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
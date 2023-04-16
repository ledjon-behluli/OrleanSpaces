using Orleans.Concurrency;
using OrleanSpaces.Tuples.Typed.Numerics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct DateTimeTuple : ISpaceTuple<DateTime, DateTimeTuple>
{
    private readonly DateTime[] fields;

    public DateTime this[int index] => fields[index];
    public int Length => fields.Length;

    public DateTimeTuple() : this(Array.Empty<DateTime>()) { }
    public DateTimeTuple(DateTime[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeTuple left, DateTimeTuple right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple left, DateTimeTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeTuple tuple && Equals(tuple);

    public bool Equals(DateTimeTuple other) => this.SequentialEquals(other);

    public int CompareTo(DateTimeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct DateTimeTuple_Optimized_V1 : INumericTuple<long, DateTimeTuple_Optimized_V1>
{
    private readonly long[] fields;

    public long this[int index] => fields[index];
    public int Length => fields.Length;

    Span<long> INumericTuple<long, DateTimeTuple_Optimized_V1>.Data => fields.AsSpan();

    public DateTimeTuple_Optimized_V1() : this(Array.Empty<DateTime>()) { }
    public DateTimeTuple_Optimized_V1(DateTime[] fields)
    {
        this.fields = new long[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            this.fields[i] = fields[i].Ticks;
        }
    }

    public static bool operator ==(DateTimeTuple_Optimized_V1 left, DateTimeTuple_Optimized_V1 right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple_Optimized_V1 left, DateTimeTuple_Optimized_V1 right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeTuple_Optimized_V1 tuple && Equals(tuple);

    public bool Equals(DateTimeTuple_Optimized_V1 other) => this.ParallelEquals(other);

    public int CompareTo(DateTimeTuple_Optimized_V1 other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct DateTimeTuple_Optimized_V2 : INumericTuple<long, DateTimeTuple_Optimized_V2>
{
    private readonly DateTime[] fields;

    public DateTime this[int index] => fields[index];
    public int Length => fields.Length;

    long ISpaceTuple<long, DateTimeTuple_Optimized_V2>.this[int index] => this[index].Ticks;
    Span<long> INumericTuple<long, DateTimeTuple_Optimized_V2>.Data => MemoryMarshal.Cast<DateTime, long>(fields);

    public DateTimeTuple_Optimized_V2() : this(Array.Empty<DateTime>()) { }
    public DateTimeTuple_Optimized_V2(DateTime[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeTuple_Optimized_V2 left, DateTimeTuple_Optimized_V2 right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple_Optimized_V2 left, DateTimeTuple_Optimized_V2 right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeTuple_Optimized_V2 tuple && Equals(tuple);

    public bool Equals(DateTimeTuple_Optimized_V2 other) => this.ParallelEquals(other);

    public int CompareTo(DateTimeTuple_Optimized_V2 other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

//todo: pick this one, and write some benchmarks (permanents) that show the diff
[Immutable]
public readonly struct DateTimeTuple_Optimized_V3 : ISpaceTuple<DateTime, DateTimeTuple_Optimized_V3>
{
    private readonly DateTime[] fields;

    public DateTime this[int index] => fields[index];
    public int Length => fields.Length;

    public DateTimeTuple_Optimized_V3() : this(Array.Empty<DateTime>()) { }
    public DateTimeTuple_Optimized_V3(DateTime[] fields) => this.fields = fields;

    public static bool operator ==(DateTimeTuple_Optimized_V3 left, DateTimeTuple_Optimized_V3 right) => left.Equals(right);
    public static bool operator !=(DateTimeTuple_Optimized_V3 left, DateTimeTuple_Optimized_V3 right) => !(left == right);

    public override bool Equals(object? obj) => obj is DateTimeTuple_Optimized_V3 tuple && Equals(tuple);

    public bool Equals(DateTimeTuple_Optimized_V3 other)
    {
        LongTuple thisTicks = new(MemoryMarshal.Cast<DateTime, long>(fields).ToArray());
        LongTuple otherTicks = new(MemoryMarshal.Cast<DateTime, long>(other.fields).ToArray());

        return thisTicks.ParallelEquals(otherTicks);
    }

    public int CompareTo(DateTimeTuple_Optimized_V3 other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}
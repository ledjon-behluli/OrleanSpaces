﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class DecimalTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateDecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new DecimalTuple(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_DecimalTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    #endregion

    #region Equality Type

    [BenchmarkCategory("Equality Type", "SpaceTuple"), Benchmark]
    public void SpaceTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
               1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential DecimalTuple"), Benchmark]
    public void SequentialDecimalTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialDecimalTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel DecimalTuple"), Benchmark]
    public void ParallelDecimalTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            DecimalTuple tuple = new(
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m,
                1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m, 1111111.111m);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialDecimalTuple : ISpaceTuple<decimal, SequentialDecimalTuple>
    {
        private readonly decimal[] fields;

        public decimal this[int index] => fields[index];
        public int Length => fields.Length;

        public SequentialDecimalTuple() : this(Array.Empty<decimal>()) { }
        public SequentialDecimalTuple(params decimal[] fields) => this.fields = fields;

        public bool Equals(SequentialDecimalTuple other) => this.SequentialEquals(other);

        public ReadOnlySpan<bool> AsSpan() => throw new NotImplementedException();
        public int CompareTo(SequentialDecimalTuple other) => throw new NotImplementedException();
        public override string ToString() => throw new NotImplementedException();
        public bool TryFormat(Span<char> destination, out int charsWritten) => throw new NotImplementedException();
    }

    #endregion
}
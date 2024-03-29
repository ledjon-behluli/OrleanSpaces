﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class ShortTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(1, 2, 3, 4, 5, 6, 7, 8);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateShortTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new ShortTuple(new short[] { 1, 2, 3, 4, 5, 6, 7, 8 });
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4 });
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            tuple.Equals(tuple);
        }
    }
   
    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64 });
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_ShortTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128 });
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
            SpaceTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential ShortTuple"), Benchmark]
    public void SequentialShortTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialShortTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel ShortTuple"), Benchmark]
    public void ParallelShortTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            ShortTuple tuple = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32);
            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialShortTuple : ISpaceTuple<short>, IEquatable<SequentialShortTuple>
    {
        private readonly short[] fields;

        public ref readonly short this[int index] => ref fields[index];
        public int Length => fields?.Length ?? 0;
        public bool IsEmpty => Length == 0;

        public SequentialShortTuple(params short[] fields) => this.fields = fields;

        public bool Equals(SequentialShortTuple other) => this.SequentialEquals<short, SequentialShortTuple>(other);

        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
        public ReadOnlySpan<short>.Enumerator GetEnumerator() => throw new NotImplementedException();
    }

    #endregion
}
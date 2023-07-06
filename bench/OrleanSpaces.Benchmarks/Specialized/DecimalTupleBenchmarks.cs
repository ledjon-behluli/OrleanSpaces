using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

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

    private readonly struct SequentialDecimalTuple : ISpaceTuple<decimal>, IEquatable<SequentialDecimalTuple>
    {
        private readonly decimal[] fields;

        public ref readonly decimal this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialDecimalTuple(params decimal[] fields) => this.fields = fields;

        public bool Equals(SequentialDecimalTuple other) => this.SequentialEquals(other);

        ISpaceTemplate<decimal> ISpaceTuple<decimal>.ToTemplate() => throw new NotImplementedException();
        static ISpaceTuple<decimal> ISpaceTuple<decimal>.Create(decimal[] fields) => throw new NotImplementedException();
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
        public ReadOnlySpan<decimal>.Enumerator GetEnumerator() => throw new NotImplementedException();
    }

    #endregion
}
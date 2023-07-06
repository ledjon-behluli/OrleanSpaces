using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class DoubleTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateDoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new DoubleTuple(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111, 1111111.111, 1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(1111111.111, 1111111.111, 1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_DoubleTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

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
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
               1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential DoubleTuple"), Benchmark]
    public void SequentialDoubleTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialDoubleTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel DoubleTuple"), Benchmark]
    public void ParallelDoubleTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            DoubleTuple tuple = new(
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111,
                1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111, 1111111.111);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialDoubleTuple : ISpaceTuple<double>, IEquatable<SequentialDoubleTuple>
    {
        private readonly double[] fields;

        public ref readonly double this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialDoubleTuple(params double[] fields) => this.fields = fields;

        public bool Equals(SequentialDoubleTuple other) => this.SequentialEquals(other);

        ISpaceTemplate<double> ISpaceTuple<double>.ToTemplate() => throw new NotImplementedException();
        static ISpaceTuple<double> ISpaceTuple<double>.Create(double[] fields) => throw new NotImplementedException();
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
        public ReadOnlySpan<double>.Enumerator GetEnumerator() => throw new NotImplementedException();
    }

    #endregion
}
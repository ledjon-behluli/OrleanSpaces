using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class FloatTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateFloatTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new FloatTuple(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_FloatTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

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
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
               1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential FloatTuple"), Benchmark]
    public void SequentialFloatTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialFloatTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel FloatTuple"), Benchmark]
    public void ParallelFloatTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            FloatTuple tuple = new(
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f,
                1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f, 1111111.111f);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialFloatTuple : ISpaceTuple<float>, IEquatable<SequentialFloatTuple>
    {
        private readonly float[] fields;

        public ref readonly float this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialFloatTuple(params float[] fields) => this.fields = fields;

        public bool Equals(SequentialFloatTuple other) => this.SequentialEquals(other);

        ISpaceTemplate<float> ISpaceTuple<float>.ToTemplate() => throw new NotImplementedException();
        static ISpaceTuple<float> ISpaceTuple<float>.Create(float[] fields) => throw new NotImplementedException();
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
        public ReadOnlySpan<float>.Enumerator GetEnumerator() => throw new NotImplementedException();
    }

    #endregion
}
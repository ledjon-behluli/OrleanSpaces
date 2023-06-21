using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class HugeTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateHugeTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new HugeTuple(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_HugeTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

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
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
               Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential HugeTuple"), Benchmark]
    public void SequentialHugeTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialHugeTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel HugeTuple"), Benchmark]
    public void ParallelHugeTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            HugeTuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialHugeTuple : ISpaceTuple<Int128>, IEquatable<SequentialHugeTuple>
    {
        private readonly Int128[] fields;

        public ref readonly Int128 this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialHugeTuple(params Int128[] fields) => this.fields = fields;

        public bool Equals(SequentialHugeTuple other) => this.SequentialEquals(other);
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
        public ReadOnlySpan<Int128>.Enumerator GetEnumerator() => throw new NotImplementedException();
    }

    #endregion
}
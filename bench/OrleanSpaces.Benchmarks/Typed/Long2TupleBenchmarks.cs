using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class Long2TupleBenchmarks
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
    public void InstantiateLong2Tuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new Long2Tuple(
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
    public void XS_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(Int128.MaxValue, Int128.MaxValue);
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
    public void S_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
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
    public void M_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);
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
    public void L_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(
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
    public void XL_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(
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
    public void XXL_Long2Tuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(
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

    [BenchmarkCategory("Equality Type", "Sequential Long2Tuple"), Benchmark]
    public void SequentialLong2TupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialLong2Tuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel Long2Tuple"), Benchmark]
    public void ParallelLong2TupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            Long2Tuple tuple = new(
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue,
                Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue, Int128.MaxValue);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialLong2Tuple : ISpaceTuple<Int128, SequentialLong2Tuple>
    {
        private readonly Int128[] fields;

        public ref readonly Int128 this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialLong2Tuple(params Int128[] fields) => this.fields = fields;

        public bool Equals(SequentialLong2Tuple other) => this.SequentialEquals(other);
        public int CompareTo(SequentialLong2Tuple other) => throw new NotImplementedException();
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
    }

    #endregion
}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TimeSpanTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateTimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new TimeSpanTuple(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_TimeSpanTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

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
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
               TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential TimeSpanTuple"), Benchmark]
    public void SequentialTimeSpanTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialTimeSpanTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel TimeSpanTuple"), Benchmark]
    public void ParallelTimeSpanTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            TimeSpanTuple tuple = new(
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero,
                TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialTimeSpanTuple : ISpaceTuple<TimeSpan, SequentialTimeSpanTuple>
    {
        private readonly TimeSpan[] fields;

        public ref readonly TimeSpan this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialTimeSpanTuple() : this(Array.Empty<TimeSpan>()) { }
        public SequentialTimeSpanTuple(params TimeSpan[] fields) => this.fields = fields;

        public bool Equals(SequentialTimeSpanTuple other) => this.SequentialEquals(other);
        public int CompareTo(SequentialTimeSpanTuple other) => throw new NotImplementedException();
    }

    #endregion
}
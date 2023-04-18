using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class DateTimeOffsetTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateDateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new DateTimeOffsetTuple(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(DateTime.Now, DateTime.Now); 
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(DateTime.Now, DateTime.Now);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);


            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_DateTimeOffsetTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

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
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
               DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential DateTimeOffsetTuple"), Benchmark]
    public void SequentialDateTimeOffsetTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialDateTimeOffsetTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel DateTimeOffsetTuple"), Benchmark]
    public void ParallelDateTimeOffsetTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            DateTimeOffsetTuple tuple = new(
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialDateTimeOffsetTuple : ISpaceTuple<DateTime, SequentialDateTimeOffsetTuple>
    {
        private readonly DateTime[] fields;

        public DateTime this[int index] => fields[index];
        public int Length => fields.Length;

        public SequentialDateTimeOffsetTuple() : this(Array.Empty<DateTime>()) { }
        public SequentialDateTimeOffsetTuple(params DateTime[] fields) => this.fields = fields;

        public static bool operator ==(SequentialDateTimeOffsetTuple left, SequentialDateTimeOffsetTuple right) => left.Equals(right);
        public static bool operator !=(SequentialDateTimeOffsetTuple left, SequentialDateTimeOffsetTuple right) => !(left == right);

        public override bool Equals(object obj) => obj is SequentialDateTimeOffsetTuple tuple && Equals(tuple);

        public bool Equals(SequentialDateTimeOffsetTuple other) => this.SequentialEquals(other);

        public int CompareTo(SequentialDateTimeOffsetTuple other) => Length.CompareTo(other.Length);

        public override int GetHashCode() => fields.GetHashCode();

        public override string ToString() => $"({string.Join(", ", fields)})";
    }

    #endregion
}

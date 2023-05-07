using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class CharTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateCharTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new CharTuple(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new('a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new('a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new('a', 'a', 'a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new('a', 'a', 'a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_CharTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

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
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
               'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential CharTuple"), Benchmark]
    public void SequentialCharTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialCharTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel CharTuple"), Benchmark]
    public void ParallelCharTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            CharTuple tuple = new(
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
                'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialCharTuple : ISpaceTuple<char, SequentialCharTuple>
    {
        private readonly char[] fields;

        public ref readonly char this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialCharTuple() : this(Array.Empty<char>()) { }
        public SequentialCharTuple(params char[] fields) => this.fields = fields;

        public bool Equals(SequentialCharTuple other) => this.SequentialEquals(other);
        public int CompareTo(SequentialCharTuple other) => throw new NotImplementedException();
    }

    #endregion
}

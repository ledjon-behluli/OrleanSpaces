using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples.Typed;
using OrleanSpaces.Tuples;

[ShortRunJob] //TODO: Remove
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class EnumTupleBenchmarks
{
    private const int iterations = 100_000;
    private enum MyEnum : byte { A = 0 }

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateSpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new SpaceTuple(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateEnumTuple()
    {
        for (int i = 0; i < iterations; i++)
            _ = new EnumTuple<MyEnum>(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
    }

    #endregion

    #region Equality

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XS"), Benchmark]
    public void XS_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "S"), Benchmark]
    public void S_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "M"), Benchmark]
    public void M_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);
            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
               MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
               MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "L"), Benchmark]
    public void L_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XL"), Benchmark]
    public void XL_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_SpaceTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            SpaceTuple tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality", "XXL"), Benchmark]
    public void XXL_EnumTuple()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

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
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Sequential EnumTuple"), Benchmark]
    public void SequentialEnumTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            SequentialEnumTuple tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    [BenchmarkCategory("Equality Type", "Parallel EnumTuple"), Benchmark]
    public void ParallelEnumTupleEquality()
    {
        for (int i = 0; i < iterations; i++)
        {
            EnumTuple<MyEnum> tuple = new(
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A,
                MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A, MyEnum.A);

            tuple.Equals(tuple);
        }
    }

    private readonly struct SequentialEnumTuple : ISpaceTuple<MyEnum>, IEquatable<SequentialEnumTuple>
    {
        private readonly MyEnum[] fields;

        public ref readonly MyEnum this[int index] => ref fields[index];
        public int Length => fields.Length;

        public SequentialEnumTuple(params MyEnum[] fields) => this.fields = fields;

        public bool Equals(SequentialEnumTuple other) => this.SequentialEquals(other);
        public ReadOnlySpan<char> AsSpan() => throw new NotImplementedException();
    }

    #endregion
}
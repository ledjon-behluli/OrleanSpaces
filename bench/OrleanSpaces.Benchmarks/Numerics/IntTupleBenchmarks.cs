using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Numerics;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class IntTupleBenchmarks
{
    private const int iterations = 100_000;

    private readonly static int[] fields = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
    private readonly static SpaceTuple spaceTuple = new(fields);
    private readonly static IntTuple intTuple = new(fields);

    [BenchmarkCategory("Init"), Benchmark]
    public void SpaceTuple_Init()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = new SpaceTuple(fields);
        }
    }

    [BenchmarkCategory("Init"), Benchmark]
    public void IntTuple_Init()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = new IntTuple(fields);
        }
    }
   
    [BenchmarkCategory("Equals", "SpaceTuple"), Benchmark]
    public void SpaceTuple_Equals()
    {
        for (int i = 0; i < iterations; i++)
            spaceTuple.Equals(spaceTuple);
    }

    [BenchmarkCategory("Equals", "IntTuple"), Benchmark]
    public void IntTuple_Equals()
    {
        for (int i = 0; i < iterations; i++)
            intTuple.Equals(intTuple);
    }
}

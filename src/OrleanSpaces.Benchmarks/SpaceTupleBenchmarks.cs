using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SpaceTupleBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateNullWithNew()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = new SpaceTuple();
        }
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateNullWithRef()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = SpaceTuple.Passive;
        }
    }

    #endregion

    #region Equality

    private readonly static SpaceTuple baseTuple = new(1);
    private readonly static SpaceTuple shortTuple = new(1, "a");
    private readonly static SpaceTuple mediumTuple = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);
    private readonly static SpaceTuple longTuple = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTuple.Equals(shortTuple);
    }

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTuple.Equals(baseTuple);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTuple.Equals(mediumTuple);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTuple.Equals(baseTuple);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTuple.Equals(longTuple);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTuple.Equals(baseTuple);
    }

    #endregion
}
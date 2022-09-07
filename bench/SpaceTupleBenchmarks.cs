using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
public class SpaceTupleBenchmarks
{
    private const int iterations = 100_000;

    private readonly static SpaceTuple baseTuple = SpaceTuple.Create(1);
    private readonly static SpaceTuple shortTuple = SpaceTuple.Create((1, "a"));
    private readonly static SpaceTuple mediumTuple = SpaceTuple.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    private readonly static SpaceTuple longTuple = SpaceTuple.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

    [Benchmark]
    public void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTuple.Equals(shortTuple);
    }

    [Benchmark]
    public void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTuple.Equals(baseTuple);
    }

    [Benchmark]
    public void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTuple.Equals(mediumTuple);
    }

    [Benchmark]
    public void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTuple.Equals(baseTuple);
    }

    [Benchmark]
    public void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTuple.Equals(longTuple);
    }

    [Benchmark]
    public void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTuple.Equals(baseTuple);
    }
}
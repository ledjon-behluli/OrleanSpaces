using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
public class SpaceUnitBenchmarks
{
    private const int iterations = 100_000;
    private static readonly SpaceUnit[] units = new SpaceUnit[iterations];

    [Benchmark]
    public void ArrayWithOneItem()
    {
        units[0] = SpaceUnit.Null;
    }

    [Benchmark]
    public void ArrayWithMultipleItems()
    {
        for (int i = 0; i < iterations; i++)
            units[i] = SpaceUnit.Null;
    }
}

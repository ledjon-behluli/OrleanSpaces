using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SpaceUnitBenchmarks
{
    private const int iterations = 100_000;

    private static readonly SpaceUnit[] unitStructs = new SpaceUnit[iterations];
    private static readonly SpaceUnitClass[] unitClasses = new SpaceUnitClass[iterations];

    [Benchmark]
    public void UnitStructFillArray()
    {
        for (int i = 0; i < iterations; i++)
        {
            unitStructs[i] = SpaceUnit.Null;
        }
    }

    [Benchmark]
    public void UnitClassFillArray()
    {
        for (int i = 0; i < iterations; i++)
        {
            unitClasses[i] = SpaceUnitClass.Null;
        }
    }

    private class SpaceUnitClass
    {
        private static readonly SpaceUnitClass @null = new();
        public static ref readonly SpaceUnitClass Null => ref @null;
    }
}
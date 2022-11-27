using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SpaceUnitBenchmarks
{
    private const int iterations = 100_000;

    private static readonly SpaceUnit[] unitStructs = new SpaceUnit[iterations];
    private static readonly SpaceUnitClass[] unitClasses = new SpaceUnitClass[iterations];

    [Benchmark]
    public void FillArrayOfUnitStruct()
    {
        for (int i = 0; i < iterations; i++)
        {
            unitStructs[i] = new();
        }
    }

    [Benchmark]
    public void FillArrayOfUnitClass()
    {
        for (int i = 0; i < iterations; i++)
        {
            unitClasses[i] = new();
        }
    }

    private class SpaceUnitClass : ITuple, IEquatable<SpaceUnitClass>, IComparable<SpaceUnitClass>
    {
        int ITuple.Length => 1;
        object ITuple.this[int index] => index == 0 ? this : throw new IndexOutOfRangeException();

        public override bool Equals(object obj) => obj is SpaceUnitClass;
        public bool Equals(SpaceUnitClass other) => true;
        public int CompareTo(SpaceUnitClass other) => 0;
        public override int GetHashCode() => 0;
    }
}
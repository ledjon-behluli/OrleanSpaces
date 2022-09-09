using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class SpaceUnitBenchmarks
{
    private const int iterations = 100_000;

    private const string structCategory = "Ref Struct";
    private const string classCategory = "Class";

    private static readonly SpaceUnit[] unitStructs = new SpaceUnit[iterations];
    private static readonly SpaceUnitClass[] unitClasses = new SpaceUnitClass[iterations];

    [BenchmarkCategory(structCategory), Benchmark]
    public void InsertFirstArrayItem_RefStruct()
    {
        unitStructs[0] = SpaceUnit.Null;
    }

    [BenchmarkCategory(structCategory), Benchmark]
    public void InsertAllArrayItems_RefStruct()
    {
        for (int i = 0; i < iterations; i++)
        {
            unitStructs[i] = SpaceUnit.Null;
        }
    }

    [BenchmarkCategory(classCategory), Benchmark]
    public void InsertFirstTestArrayItem_Class()
    {
        unitClasses[0] = SpaceUnitClass.Null;
    }

    [BenchmarkCategory(classCategory), Benchmark]
    public void InsertAllTestArrayItems_Class()
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

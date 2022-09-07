using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OrleanSpaces.Primitives;

//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
//BenchmarkRunner.Run<TaskPartitionerBenchmarks>();

BenchmarkRunner.Run<SpaceTemplateBenchmarks1>();

Console.ReadKey();



[MemoryDiagnoser]
public class SpaceTemplateBenchmarks1
{
    private const int iterations = 10;
        //1_000_000;

    private readonly static SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f, 1, "a", 1.5f, 1, "a", 1.5f, 1, "a", 1.5f));
    private readonly static SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f, 1, "a", 1.5f, 1, "a", 1.5f, 1, "a", 1.5f));

    [Benchmark]
    public void FullMatch_Uni()
    {
        for (int i = 0; i < iterations; i++)
            template1.IsSatisfiedBy(tuple);
    }

    [Benchmark]
    public void FullMatch_Bi()
    {
        for (int i = 0; i < iterations; i++)
            template1.IsSatisfiedByTraverseBothSides(tuple);
    }
}
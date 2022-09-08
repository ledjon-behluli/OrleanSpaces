using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OrleanSpaces.Primitives;

//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
//BenchmarkRunner.Run<TaskPartitionerBenchmarks>();

BenchmarkRunner.Run<TestBench>();

Console.ReadKey();

[MemoryDiagnoser]
[ShortRunJob]
public class TestBench
{
    private readonly static SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f));

    [Benchmark]
    public void Normal()
    {
        for (int i = 0; i < 10_000; i++)
            template.IsSatisfiedBy(tuple);
    }

    [Benchmark]
    public void ByRef()
    {
        for (int i = 0; i < 10_000; i++)
            template.IsSatisfiedByRef(tuple);
    }
}
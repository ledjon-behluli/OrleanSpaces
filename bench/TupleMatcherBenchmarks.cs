using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

[MemoryDiagnoser]
[ShortRunJob]
public class TupleMatcherBenchmarks
{
    private const int iterations = 100_000;

    private readonly static SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template2 = SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f));
    private readonly static SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 2.5f));
    private readonly static SpaceTemplate template4 = SpaceTemplate.Create((1, "a"));

    [Benchmark]
    public void FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            TupleMatcher.IsMatch(tuple, template1);
    }

    [Benchmark]
    public void PartialMatch()
    {
        for (int i = 0; i < iterations; i++)
            TupleMatcher.IsMatch(tuple, template2);
    }

    [Benchmark]
    public void NoMatchSameLengths()
    {
        for (int i = 0; i < iterations; i++)
            TupleMatcher.IsMatch(tuple, template3);
    }

    [Benchmark]
    public void NoMatchDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            TupleMatcher.IsMatch(tuple, template4);
    }
}

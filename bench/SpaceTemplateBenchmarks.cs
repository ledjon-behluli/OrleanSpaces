using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class SpaceTemplateBenchmarks
{
    private const int iterations = 100_000;

    #region Equality

    private readonly static SpaceTemplate baseTemplate = SpaceTemplate.Create(1);
    private readonly static SpaceTemplate shortTemplate = SpaceTemplate.Create((1, "a"));
    private readonly static SpaceTemplate mediumTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    private readonly static SpaceTemplate longTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(shortTemplate);
    }

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(mediumTemplate);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(longTemplate);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(baseTemplate);
    }

    #endregion

    #region Satisfaction

    private readonly static SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template2 = SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f));
    private readonly static SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 2.5f));
    private readonly static SpaceTemplate template4 = SpaceTemplate.Create((1, "a"));

    [BenchmarkCategory("Satisfaction"), Benchmark]
    public void FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            template1.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory("Satisfaction"), Benchmark]
    public void PartialMatch()
    {
        for (int i = 0; i < iterations; i++)
            template2.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory("Satisfaction"), Benchmark]
    public void NoMatchSameLengths()
    {
        for (int i = 0; i < iterations; i++)
            template3.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory("Satisfaction"), Benchmark]
    public void NoMatchDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            template4.IsSatisfiedBy(tuple);
    }

    #endregion
}
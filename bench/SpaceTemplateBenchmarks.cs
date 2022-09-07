using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class SpaceTemplateBenchmarks
{
    private const int iterations = 100_000;

    #region Template Equality

    private const string equalityCategory = "Template Equality";
    private readonly static SpaceTemplate baseTemplate = SpaceTemplate.Create(1);
    private readonly static SpaceTemplate shortTemplate = SpaceTemplate.Create((1, "a"));
    private readonly static SpaceTemplate mediumTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    private readonly static SpaceTemplate longTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(shortTemplate);
    }

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(mediumTemplate);
    }

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(longTemplate);
    }

    [BenchmarkCategory(equalityCategory), Benchmark]
    public static void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(baseTemplate);
    }

    #endregion

    #region Tuple Satisfaction

    private const string satisfactionCategory = "Tuple Satisfaction";
    private readonly static SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f));
    private readonly static SpaceTemplate template2 = SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f));
    private readonly static SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 2.5f));
    private readonly static SpaceTemplate template4 = SpaceTemplate.Create((1, "a"));

    [BenchmarkCategory(satisfactionCategory), Benchmark]
    public static void FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            template1.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory(satisfactionCategory), Benchmark]
    public static void PartialMatch()
    {
        for (int i = 0; i < iterations; i++)
            template2.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory(satisfactionCategory), Benchmark]
    public static void NoMatchSameLengths()
    {
        for (int i = 0; i < iterations; i++)
            template3.IsSatisfiedBy(tuple);
    }

    [BenchmarkCategory(satisfactionCategory), Benchmark]
    public static void NoMatchDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            template4.IsSatisfiedBy(tuple);
    }

    #endregion
}

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class SpaceTemplateBenchmarksTest
{
    private const int iterations = 10_000;

    private readonly static SpaceTuple smallTuple = SpaceTuple.Create((1, 1, 1, 1, 1));
    private readonly static SpaceTuple largeTuple = SpaceTuple.Create(
        (1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
         1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
         1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
         1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
         1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
         1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));

    private readonly static SpaceTemplate smallTemplate = SpaceTemplate.Create(smallTuple);
    private readonly static SpaceTemplate largeTemplate = SpaceTemplate.Create(largeTuple);

    [Benchmark]
    public void Small_FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            smallTemplate.IsSatisfiedBy(smallTuple);
    }

    [Benchmark]
    public void Small_FullMatch_Span()
    {
        for (int i = 0; i < iterations; i++)
            smallTemplate.IsSatisfiedBySpan(smallTuple);
    }

    [Benchmark]
    public void Large_FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            largeTemplate.IsSatisfiedBy(largeTuple);
    }

    [Benchmark]
    public void Large_FullMatch_Span()
    {
        for (int i = 0; i < iterations; i++)
            largeTemplate.IsSatisfiedBySpan(largeTuple);
    }
}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SpaceTemplateBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateByCtor()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = new SpaceTemplate(SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
            _ = new SpaceTemplate(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
        }
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateByRefReturn()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = SpaceTemplateCache.Singlet;
            _ = SpaceTemplateCache.Pair;
            _ = SpaceTemplateCache.Triple;
            _ = SpaceTemplateCache.Quadruple;
            _ = SpaceTemplateCache.Quintuple;
            _ = SpaceTemplateCache.Sextuple;
            _ = SpaceTemplateCache.Septuple;
            _ = SpaceTemplateCache.Octuple;
        }
    }

    #endregion

    #region Equality

    private readonly static SpaceTemplate baseTemplate = new(1);
    private readonly static SpaceTemplate shortTemplate = new(1, "a");
    private readonly static SpaceTemplate mediumTemplate = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);
    private readonly static SpaceTemplate longTemplate = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);

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

    #region Matching

    private readonly static SpaceTuple tuple = new(1, "a", 1.5f);
    private readonly static SpaceTemplate template1 = new(1, "a", 1.5f);
    private readonly static SpaceTemplate template2 = new(1, SpaceUnit.Null, 1.5f);
    private readonly static SpaceTemplate template3 = new(1, "a", 2.5f);
    private readonly static SpaceTemplate template4 = new(1, "a");

    [BenchmarkCategory("Matching"), Benchmark]
    public void FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            template1.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void PartialMatch()
    {
        for (int i = 0; i < iterations; i++)
            template2.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void NoMatchSameLengths()
    {
        for (int i = 0; i < iterations; i++)
            template3.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void NoMatchDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            template4.Matches(tuple);
    }

    #endregion
}

public readonly struct SpaceTemplateCache
{
    private static readonly SpaceTemplate singlet = new(SpaceUnit.Null);
    private static readonly SpaceTemplate pair = new(SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate triple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate quadruple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate quintuple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate sextuple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate septuple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate octuple = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);

    public static ref readonly SpaceTemplate Singlet => ref singlet;
    public static ref readonly SpaceTemplate Pair => ref pair;
    public static ref readonly SpaceTemplate Triple => ref triple;
    public static ref readonly SpaceTemplate Quadruple => ref quadruple;
    public static ref readonly SpaceTemplate Quintuple => ref quintuple;
    public static ref readonly SpaceTemplate Sextuple => ref sextuple;
    public static ref readonly SpaceTemplate Septuple => ref septuple;
    public static ref readonly SpaceTemplate Octuple => ref octuple;
}
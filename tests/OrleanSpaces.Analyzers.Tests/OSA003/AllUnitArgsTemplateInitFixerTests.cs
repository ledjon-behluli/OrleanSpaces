using OrleanSpaces.Analyzers.OSA003;

namespace OrleanSpaces.Analyzers.Tests.OSA003;

public class AllUnitArgsTemplateInitFixerTests : FixerFixture
{
    public AllUnitArgsTemplateInitFixerTests() : base(
        new AllUnitArgsTemplateInitAnalyzer(),
        new AllUnitArgsTemplateInitFixer(),
        AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal() =>
        Assert.Equal("OSA003", provider.FixableDiagnosticIds.Single());

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null)|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    public void Should_Fix_SpaceTemplate(int numOfSpaceUnits, string code)
    {
        TestCodeFix(code, GenerateFixedCode(numOfSpaceUnits), Namespace.OrleanSpaces_Tuples);
        static string GenerateFixedCode(int numOfSpaceUnits)
        {
            string unitArrayArgument = string.Join(", ", Enumerable.Repeat("SpaceUnit.Null", numOfSpaceUnits));

            return
    @$"namespace MyNamespace; SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfSpaceUnits};

public readonly struct SpaceTemplateCache
{{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_{numOfSpaceUnits} = new({unitArrayArgument});
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_{numOfSpaceUnits} => ref tuple_{numOfSpaceUnits};
}}";
        }
    }

    [Fact]
    public void Should_Fix_1_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string cacheCode = @"
public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}";

        string code =
@$"namespace MyNamespace; 

SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

{cacheCode}";

        string fix =
@$"namespace MyNamespace; 

SpaceTemplate template = SpaceTemplateCache.Tuple_1;

{cacheCode}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string cacheCode = @"
public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string code = 
@$"namespace MyNamespace; 

SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

{cacheCode}";

        string fix =
@$"namespace MyNamespace; 

SpaceTemplate template = SpaceTemplateCache.Tuple_2;

{cacheCode}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_With_FullyQualifiedName_By_Using_Existing_SpaceTemplateCache()
    {
        string cacheCode = @"
public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly OrleanSpaces.Tuples.SpaceTemplate tuple_2 = new (SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string code =
@$"namespace MyNamespace; 

SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

{cacheCode}";

        string fix =
@$"namespace MyNamespace; 

SpaceTemplate template = SpaceTemplateCache.Tuple_2;

{cacheCode}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }
}

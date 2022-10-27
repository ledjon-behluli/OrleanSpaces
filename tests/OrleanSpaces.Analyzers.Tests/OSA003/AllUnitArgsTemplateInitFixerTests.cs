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

    [Fact]
    public void Should_Fix_SpaceTemplate_1() =>
        TestCodeFix(
"SpaceTemplate template = [|new()|];",
@"SpaceTemplate template = SpaceTemplateFactory.Tuple_1;

public readonly struct SpaceTemplateFactory
{
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}", Namespace.OrleanSpaces_Tuples);

    [Fact]
    public void Should_Fix_SpaceTemplate_2() =>
       TestCodeFix(
"SpaceTemplate template = [|new(SpaceUnit.Null)|];",
@"SpaceTemplate template = SpaceTemplateFactory.Tuple_1;

public readonly struct SpaceTemplateFactory
{
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}", Namespace.OrleanSpaces_Tuples);

    [Fact]
    public void Should_Fix_SpaceTemplate_3() =>
       TestCodeFix(
@"namespace MyNamespace;
SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];",

@"namespace MyNamespace;
SpaceTemplate template = SpaceTemplateFactory.Tuple_2;

public readonly struct SpaceTemplateFactory
{
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}", Namespace.OrleanSpaces_Tuples);
}

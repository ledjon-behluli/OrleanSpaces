using OrleanSpaces.Analyzers.Analyzers;
using OrleanSpaces.Analyzers.Fixers;

namespace OrleanSpaces.Analyzers.Tests.Fixers;

public class DefaultableTypeInitializationFixerTests : Fixture
{
    public DefaultableTypeInitializationFixerTests() : base(
        new DefaultableTypeInitializationAnalyzer(),
        new DefaultableTypeInitializationFixer(),
        DefaultableTypeInitializationAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OSA001", provider.FixableDiagnosticIds.Single());
    }

    [Theory]
    [InlineData("SpaceUnit unit = [|new SpaceUnit()|];")]
    [InlineData("SpaceUnit unit = [|new()|];")]
    [InlineData("SpaceUnit unit = [|default(SpaceUnit)|];")]
    [InlineData("SpaceUnit unit = [|default|];")]
    public void Should_Fix_SpaceUnit(string source) =>
        TestCodeFix(ComposeMarkup(source), ComposeMarkup("SpaceUnit unit = SpaceUnit.Null;"));

    [Theory]
    [InlineData("SpaceTuple tuple = [|new SpaceTuple()|];")]
    [InlineData("SpaceTuple tuple = [|new()|];")]
    [InlineData("SpaceTuple tuple = [|default(SpaceTuple)|];")]
    [InlineData("SpaceTuple tuple = [|default|];")]
    public void Should_Fix_SpaceTuple(string source) =>
        TestCodeFix(ComposeMarkup(source), ComposeMarkup("SpaceTuple tuple = SpaceTuple.Null;"));

    private static string ComposeMarkup(string code) => @$"using OrleanSpaces.Tuples;{code}";
}
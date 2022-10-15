using OrleanSpaces.Analyzers.OSA001;

namespace OrleanSpaces.Analyzers.Tests.OSA001;

public class TupleRefOverInitFixerTests : CodeFixFixture
{
    public TupleRefOverInitFixerTests() : base(
        new TupleRefOverInitAnalyzer(),
        new TupleRefOverInitFixer(),
        TupleRefOverInitAnalyzer.Diagnostic.Id)
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
using OrleanSpaces.Analyzers.OSA001;

namespace OrleanSpaces.Analyzers.Tests.OSA001;

public class SpaceTupleNullRefOverInitFixerTests : FixerFixture
{
    public SpaceTupleNullRefOverInitFixerTests() : base(
        new SpaceTupleNullRefOverInitAnalyzer(),
        new SpaceTupleNullRefOverInitFixer(),
        SpaceTupleNullRefOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal() =>
        Assert.Equal("OSA001", provider.FixableDiagnosticIds.Single());

    [Theory]
    [InlineData("SpaceTuple tuple = [|new SpaceTuple()|];")]
    [InlineData("SpaceTuple tuple = [|new()|];")]
    [InlineData("SpaceTuple tuple = [|default(SpaceTuple)|];")]
    [InlineData("SpaceTuple tuple = [|default|];")]
    public void Should_Fix(string code) =>
        TestCodeFix(code, "SpaceTuple tuple = SpaceTuple.Null;", Namespace.OrleanSpaces_Tuples);
}

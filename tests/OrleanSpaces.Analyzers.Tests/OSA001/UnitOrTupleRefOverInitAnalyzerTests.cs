using OrleanSpaces.Analyzers.OSA001;

namespace OrleanSpaces.Analyzers.Tests.OSA001;

public class UnitOrTupleRefOverInitAnalyzerTests : AnalyzerFixture
{
    public UnitOrTupleRefOverInitAnalyzerTests() : base(
        new UnitOrTupleRefOverInitAnalyzer(),
        UnitOrTupleRefOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = UnitOrTupleRefOverInitAnalyzer.Diagnostic;

        Assert.Equal("OSA001", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Avoid instantiation by default constructor or expression.", diagnostic.Title);
        Assert.Equal("Avoid instantiation of '{0}' by default constructor or expression.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("SpaceUnit unit = [|new SpaceUnit()|];")]
    [InlineData("SpaceUnit unit = [|new()|];")]
    [InlineData("SpaceUnit unit = [|default(SpaceUnit)|];")]
    [InlineData("SpaceUnit unit = [|default|];")]
    public void Should_Diagnose_SpaceUnit(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTuple tuple = [|new SpaceTuple()|];")]
    [InlineData("SpaceTuple tuple = [|new()|];")]
    [InlineData("SpaceTuple tuple = [|default(SpaceTuple)|];")]
    [InlineData("SpaceTuple tuple = [|default|];")]
    public void Should_Diagnose_SpaceTuple(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTuple tuple = new(1)")]
    [InlineData("SpaceTuple tuple = new(1, \"a\")")]
    [InlineData("SpaceTuple tuple = new(1, \"a\", 1.5f)")]
    public void Should_Not_Diagnose_SpaceTuple(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);
}

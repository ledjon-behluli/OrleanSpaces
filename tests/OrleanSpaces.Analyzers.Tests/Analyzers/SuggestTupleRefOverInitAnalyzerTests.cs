using Microsoft.CodeAnalysis;
using OrleanSpaces.Analyzers.Analyzers;

namespace OrleanSpaces.Analyzers.Tests.Analyzers;

public class SuggestTupleRefOverInitAnalyzerTests : Fixture
{
    public SuggestTupleRefOverInitAnalyzerTests() : base(
        new SuggestTupleRefOverInitAnalyzer(), 
        SuggestTupleRefOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OSA001", SuggestTupleRefOverInitAnalyzer.Diagnostic.Id);
        Assert.Equal(Categories.Performance, SuggestTupleRefOverInitAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, SuggestTupleRefOverInitAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("Avoid instantiation by default constructor or expression.", SuggestTupleRefOverInitAnalyzer.Diagnostic.Title);
        Assert.Equal("Avoid instantiation of '{0}' by default constructor or expression.", SuggestTupleRefOverInitAnalyzer.Diagnostic.MessageFormat);
        Assert.True(SuggestTupleRefOverInitAnalyzer.Diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("SpaceUnit unit = [|new SpaceUnit()|];")]
    [InlineData("SpaceUnit unit = [|new()|];")]
    [InlineData("SpaceUnit unit = [|default(SpaceUnit)|];")]
    [InlineData("SpaceUnit unit = [|default|];")]
    public void Should_Diagnose_SpaceUnit(string code) =>
        HasDiagnostic(ComposeMarkup(code));

    [Theory]
    [InlineData("SpaceTuple tuple = [|new SpaceTuple()|];")]
    [InlineData("SpaceTuple tuple = [|new()|];")]
    [InlineData("SpaceTuple tuple = [|default(SpaceTuple)|];")]
    [InlineData("SpaceTuple tuple = [|default|];")]
    public void Should_Diagnose_SpaceTuple(string code) =>
        HasDiagnostic(ComposeMarkup(code));

    [Theory]
    [InlineData("SpaceTuple tuple = new(1)")]
    [InlineData("SpaceTuple tuple = new(1, \"a\")")]
    [InlineData("SpaceTuple tuple = new(1, \"a\", 1.5f)")]
    public void Should_Not_Diagnose_SpaceTuple(string code) =>
        NoDiagnostic(ComposeMarkup(code));

    private static string ComposeMarkup(string code) => @$"using OrleanSpaces.Tuples;{code}";
}

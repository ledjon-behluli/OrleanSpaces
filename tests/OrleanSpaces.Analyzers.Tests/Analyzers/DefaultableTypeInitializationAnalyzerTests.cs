using Microsoft.CodeAnalysis;
using OrleanSpaces.Analyzers.Analyzers;

namespace OrleanSpaces.Analyzers.Tests.Analyzers;

public class DefaultableTypeInitializationAnalyzerTests : Fixture
{
    public DefaultableTypeInitializationAnalyzerTests() : base(
        new DefaultableTypeInitializationAnalyzer(), 
        DefaultableTypeInitializationAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OSA001", DefaultableTypeInitializationAnalyzer.Diagnostic.Id);
        Assert.Equal(Categories.Performance, DefaultableTypeInitializationAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, DefaultableTypeInitializationAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("Avoid instantiation by default constructor or expression.", DefaultableTypeInitializationAnalyzer.Diagnostic.Title);
        Assert.Equal("Avoid instantiation of '{0}' by default constructor or expression.", DefaultableTypeInitializationAnalyzer.Diagnostic.MessageFormat);
        Assert.True(DefaultableTypeInitializationAnalyzer.Diagnostic.IsEnabledByDefault);
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

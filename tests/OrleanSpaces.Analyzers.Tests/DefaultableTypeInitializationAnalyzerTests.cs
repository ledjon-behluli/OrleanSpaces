using Microsoft.CodeAnalysis;
using OrleanSpaces.Analyzers.Tests.Fixtures;

namespace OrleanSpaces.Analyzers.Tests;

public class AlwaysFalseTupleEqualityAnalyzerTests : AnalyzerFixture
{
    public AlwaysFalseTupleEqualityAnalyzerTests()
        : base(new AlwaysFalseTupleEqualityAnalyzer(), AlwaysFalseTupleEqualityAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Test1()
    {
        const string source = @"
using OrleanSpaces.Tuples;

SpaceTuple tuple1 = new(1, 1);
SpaceTuple tuple2 = new(1, 1, 1);

[|tuple1 == tuple2|];";

        HasDiagnostic(source);
    }

//    [Fact]
//    public void Test2()
//    {
//        const string source = @"
//using OrleanSpaces.Tuples;

//SpaceTuple tuple1 = new(1, 1);
//SpaceTuple tuple2 = new(1, 1, 1);

//[|tuple1.Equals(tuple2)|];";

//        HasDiagnostic(source);
//    }
}

public class DefaultableTypeInitializationAnalyzerTests : CodeFixFixture
{
    private const string diagnosticId = "OSA001";

    public DefaultableTypeInitializationAnalyzerTests()
        : base(new DefaultableTypeInitializationAnalyzer(), new DefaultableCodeFixProvider(), DefaultableTypeInitializationAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Equals_Diagnostic()
    {
        Assert.Equal(diagnosticId, DefaultableTypeInitializationAnalyzer.Diagnostic.Id);
        Assert.Equal(diagnosticId, provider.FixableDiagnosticIds.Single());
        Assert.Equal(DiagnosticCategory.Performance, DefaultableTypeInitializationAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, DefaultableTypeInitializationAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("Avoid instantiation by default constructor or expression.", DefaultableTypeInitializationAnalyzer.Diagnostic.Title);
        Assert.Equal("Avoid instantiation of '{0}' by default constructor or expression.", DefaultableTypeInitializationAnalyzer.Diagnostic.MessageFormat);
        Assert.True(DefaultableTypeInitializationAnalyzer.Diagnostic.IsEnabledByDefault);
    }

    private static string ComposeSource(string code) => @$"
using OrleanSpaces.Tuples;

class T 
{{
    void M() 
    {{
        {code}
    }} 
}}";

    #region SpaceUnit

    [Fact]
    public void SpaceUnit_DefaultConstructor() =>
        TestCodeFix(
            source: ComposeSource("SpaceUnit unit = [|new SpaceUnit()|];"),
            fixedSource: ComposeSource("SpaceUnit unit = SpaceUnit.Null;"));

    [Fact]
    public void SpaceUnit_DefaultImplicitConstructor() =>
        TestCodeFix(
            source: ComposeSource("SpaceUnit unit = [|new()|];"),
            fixedSource: ComposeSource("SpaceUnit unit = SpaceUnit.Null;"));

    [Fact]
    public void SpaceUnit_DefaultExpression() =>
        TestCodeFix(
            source: ComposeSource("SpaceUnit unit = [|default(SpaceUnit)|];"),
            fixedSource: ComposeSource("SpaceUnit unit = SpaceUnit.Null;"));

    [Fact]
    public void SpaceUnit_DefaultImplicitExpression() =>
        TestCodeFix(
            source: ComposeSource("SpaceUnit unit = [|default|];"),
            fixedSource: ComposeSource("SpaceUnit unit = SpaceUnit.Null;"));

    #endregion

    #region SpaceTuple

    [Fact]
    public void SpaceTuple_DefaultConstructor() =>
        TestCodeFix(
            source: ComposeSource("SpaceTuple tuple = [|new SpaceTuple()|];"),
            fixedSource: ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    [Fact]
    public void SpaceTuple_DefaultImplicitConstructor() =>
        TestCodeFix(
            source: ComposeSource("SpaceTuple tuple = [|new()|];"),
            fixedSource: ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    [Fact]
    public void SpaceTuple_DefaultExpression() =>
        TestCodeFix(
            source: ComposeSource("SpaceTuple tuple = [|default(SpaceTuple)|];"),
            fixedSource: ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    [Fact]
    public void SpaceTuple_DefaultImplicitExpression() =>
        TestCodeFix(
            source: ComposeSource("SpaceTuple tuple = [|default|];"),
            fixedSource: ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    #endregion
}
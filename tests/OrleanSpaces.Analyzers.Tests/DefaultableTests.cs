using Microsoft.CodeAnalysis;
using OrleanSpaces.Analyzers.Tests.Fixtures;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableAnalyzerTests : AnalyzerFixture
{
    public DefaultableAnalyzerTests() 
        : base(new DefaultableAnalyzer(), DefaultableAnalyzer.Diagnostic.Id) 
    {
    
    }

    [Fact]
    public void Should_Create_Diagnostic()
    {
        Assert.Equal("OSA001", DefaultableAnalyzer.Diagnostic.Id);
        Assert.Equal(DiagnosticCategory.Performance, DefaultableAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, DefaultableAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("Avoid instantiation by default constructor or expression.", DefaultableAnalyzer.Diagnostic.Title);
        Assert.Equal("Avoid instantiation of '{0}' by default constructor or expression.", DefaultableAnalyzer.Diagnostic.MessageFormat);
        Assert.True(DefaultableAnalyzer.Diagnostic.IsEnabledByDefault);
    }
    
    private static string ComposeSource(string code) => @$"
using OrleanSpaces.Tuples;

class T 
{{
    void M() 
    {{
        [|{code}|]
    }} 
}}";

    #region SpaceUnit

    [Fact]
    public void Should_Not_Report_On_SpaceUnit_Ref_Instance() =>
        NoDiagnostic(ComposeSource("SpaceUnit unit = SpaceUnit.Null;"));

    [Fact]
    public void Should_Report_On_Default_SpaceUnit_Constructor() =>
        HasDiagnostic(ComposeSource("SpaceUnit unit = new SpaceUnit();"));

    [Fact]
    public void Should_Report_On_Implicit_Default_SpaceUnit_Constructor() =>
        HasDiagnostic(ComposeSource("SpaceUnit unit = new();"));

    [Fact]
    public void Should_Report_On_Default_SpaceUnit_Expression() =>
        HasDiagnostic(ComposeSource("SpaceUnit unit = default(SpaceUnit);"));

    [Fact]
    public void Should_Report_On_Implicit_Default_SpaceUnit_Expression() =>
        HasDiagnostic(ComposeSource("SpaceUnit unit = default;"));

    #endregion

    #region SpaceTuple

    [Fact]
    public void Should_Not_Report_On_SpaceTuple_Ref_Instance() =>
        NoDiagnostic(ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    [Fact]
    public void Should_Report_On_Default_SpaceTuple_Constructor() =>
        HasDiagnostic(ComposeSource("SpaceTuple tuple = new SpaceTuple();"));

    [Fact]
    public void Should_Report_On_Implicit_Default_SpaceTuple_Constructor() =>
        HasDiagnostic(ComposeSource("SpaceTuple tuple = new();"));

    [Fact]
    public void Should_Report_On_Default_SpaceTuple_Expression() =>
        HasDiagnostic(ComposeSource("SpaceTuple tuple = default(SpaceTuple);"));

    [Fact]
    public void Should_Report_On_Implicit_Default_SpaceTuple_Expression() =>
        HasDiagnostic(ComposeSource("SpaceTuple tuple = default;"));

    #endregion
}

public class DefaultableCodeFixTests : CodeFixFixture
{
    public DefaultableCodeFixTests()
        : base(new DefaultableAnalyzer(), new DefaultableCodeFixProvider(), DefaultableAnalyzer.Diagnostic.Id)
    {

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
    public void Should_Fix_On_Default_SpaceUnit_Constructor() =>
        TestCodeFix(
            source: ComposeSource("SpaceTuple tuple = [|new SpaceTuple()|];"),
            fixedSource: ComposeSource("SpaceTuple tuple = SpaceTuple.Null;"));

    #endregion
}
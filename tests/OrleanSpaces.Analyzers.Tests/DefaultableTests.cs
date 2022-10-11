using OrleanSpaces.Analyzers.Tests.Fixtures;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableTests : AnalyzerFixture
{
    public DefaultableTests() 
        : base(new DefaultableAnalyzer(), DefaultableAnalyzer.DiagnosticId) 
    {
    
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
using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableTests
{
    private static DiagnosticResult ComposeDiagnosticResult(string type, int line, int count) =>
        new DiagnosticResult("OSA001", DiagnosticSeverity.Info)
            .WithMessage($"Avoid instantiation of '{type}' by default constructor or expression.")
            .WithLocation(line, count);

    private static string ComposeSource(string code) => @$"
using OrleanSpaces.Tuples;

class TestClass 
{{
    void TestMethod() 
    {{
        {code}
    }} 
}}";

    #region SpaceUnit

    [Fact]
    public async Task Should_Report_On_Default_SpaceUnit_Constructor()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceUnit unit = new SpaceUnit();"),
            diagnostics: ComposeDiagnosticResult("SpaceUnit", 8, 26));
    }

    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceUnit_Constructor()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceUnit unit = new();"),
            diagnostics: ComposeDiagnosticResult("SpaceUnit", 8, 26));
    }

    [Fact]
    public async Task Should_Report_On_Default_SpaceUnit_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceUnit unit = default(SpaceUnit);"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 26));
    }

    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceUnit_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceUnit unit = default;"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 26));
    }

    #endregion

    #region SpaceTuple

    [Fact]
    public async Task Should_Report_On_Default_SpaceTuple_Constructor()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceTuple tuple = new SpaceTuple();"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 28));
    }



    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceTuple_Constructor()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceTuple tuple = new();"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 28));
    }

    [Fact]
    public async Task Should_Report_On_Default_SpaceTuple_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceTuple tuple = default(SpaceTuple);"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 28));
    }

    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceTuple_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceTuple tuple = default;"),
            diagnostics: ComposeDiagnosticResult("SpaceTuple", 8, 28));
    }

    #endregion
}

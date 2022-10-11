using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableTests
{
    private static DiagnosticResult ComposeDiagnosticResult(string type, int line, int count) =>
        new DiagnosticResult("OSA001", DiagnosticSeverity.Info)
            .WithMessage($"Avoid instantiation of '{type}' by default constructor or expression.")
            .WithLocation(line, count);

    private static DiagnosticResult ComposeCompilerWarning(string arg, int startLine, int startColumn, int endLine, int endColumn) =>
        DiagnosticResult
            .CompilerWarning("CS0219")
            .WithSpan(startLine, startColumn, endLine, endColumn)
            .WithArguments(arg);

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
            diagnostics: new[] 
            {
                ComposeDiagnosticResult("SpaceUnit", 8, 26),
                ComposeCompilerWarning("unit", 8, 19, 8, 23)
            });
    }

    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceUnit_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceUnit unit = default;"),
            diagnostics: new[]
            {
                ComposeDiagnosticResult("SpaceUnit", 8, 26),
                ComposeCompilerWarning("unit", 8, 19, 8, 23)
            });
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
            diagnostics: new[]
            {
                ComposeDiagnosticResult("SpaceTuple", 8, 28),
                ComposeCompilerWarning("tuple", 8, 20, 8, 25)
            });
    }

    [Fact]
    public async Task Should_Report_On_Implicit_Default_SpaceTuple_Expression()
    {
        await Verifier<DefaultableAnalyzer>.VerifyAnalyzerAsync(
            source: ComposeSource("SpaceTuple tuple = default;"),
            diagnostics: new[]
            {
                ComposeDiagnosticResult("SpaceTuple", 8, 28),
                ComposeCompilerWarning("tuple", 8, 20, 8, 25)
            });
    }

    #endregion
}

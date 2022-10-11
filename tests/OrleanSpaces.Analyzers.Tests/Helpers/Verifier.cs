using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace OrleanSpaces.Analyzers.Tests;

public static class Verifier<TAnalyzer> where TAnalyzer : DiagnosticAnalyzer, new()
{
    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] diagnostics)
    {
        var test = new Test
        {
            TestCode = source,
            CompilerDiagnostics = CompilerDiagnostics.All,
        };

        test.ExpectedDiagnostics.AddRange(diagnostics);
        return test.RunAsync();
    }

    public static Task VerifyCodeFixAsync(string source, DiagnosticResult diagnostic, string fixedSource)
        => VerifyCodeFixAsync(source, fixedSource, new[] { diagnostic });

    private static Task VerifyCodeFixAsync(string source, string fixedSource, params DiagnosticResult[] diagnostics)
    {
        var test = new Test
        {
            TestCode = source,
            FixedCode = fixedSource,
        };

        test.ExpectedDiagnostics.AddRange(diagnostics);
        return test.RunAsync();
    }

    private class Test : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = AssemblyAccessor.Instace;
            TestBehaviors |= TestBehaviors.SkipGeneratedCodeCheck;
        }

        protected override IEnumerable<CodeFixProvider> GetCodeFixProviders()
        {
            var analyzer = new TAnalyzer();
            foreach (var provider in ProviderDiscovery.GetCodeFixProviders(Language))
            {
                if (analyzer.SupportedDiagnostics.Any(diagnostic => provider.FixableDiagnosticIds.Contains(diagnostic.Id)))
                {
                    yield return provider;
                }
            }
        }
    }
}

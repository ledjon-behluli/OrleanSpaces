using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.
    AnalyzerVerifier<OrleanSpaces.Analyzers.Defaultable>;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableTests
{
    [Fact]
    public async Task Test1()
    {
        string test = @"
class C
{
    public void M1()
    {
    }
    public virtual void M2()
    {
    }
    public int M3()
    {
    }
}";
        DiagnosticResult[] expected =
        {
                Verify.Diagnostic().WithLocation(4, 17).WithArguments("M1"),
                DiagnosticResult.CompilerError("CS0161").WithLocation(12, 16).WithMessage("'C.M3()': not all code paths return a value"),
            };
        await Verify.VerifyAnalyzerAsync(test, expected);
    }
}

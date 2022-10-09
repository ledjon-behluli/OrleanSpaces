using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = OrleanSpaces.Analyzers.Test.CSharpCodeFixVerifier<
    OrleanSpaces.Analyzers.DefaultableAnalyzer,
    OrleanSpaces.Analyzers.DefaultableFixer>;

namespace OrleanSpaces.Analyzers.Tests;

public class DefaultableAnalyzerTests
{
    [Fact]
    public async Task TestMethod1()
    {
        var test = @"";
        await VerifyCS.VerifyAnalyzerAsync(test);
    }
}

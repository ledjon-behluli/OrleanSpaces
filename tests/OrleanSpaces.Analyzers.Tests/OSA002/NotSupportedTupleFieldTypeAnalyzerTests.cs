using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class NotSupportedTupleFieldTypeAnalyzerTests : AnalyzerFixture
{
    public NotSupportedTupleFieldTypeAnalyzerTests() : base(
        new NotSupportedTupleFieldTypeAnalyzer(),
        NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id)
    {

    }

    private static string ComposeMarkup(string code) => @$"using OrleanSpaces.Tuples;{code}";

    [Fact]
    public void Test1()
    {
        NoDiagnostic(ComposeMarkup("SpaceTuple tuple = [|new(1, 1.5f, 1.3d, \"a\", 'a', true)|];"));
    }

    [Fact]
    public void Test2()
    {
        HasDiagnostic(ComposeMarkup("SpaceTuple tuple = [|new(typeof(int), SpaceUnit.Null)|];"));
    }

    [Fact]
    public void Test3()
    {
        HasDiagnostic(@"
using OrleanSpaces.Tuples;

int a = 1;
SpaceTuple tuple = new[|(a)|];");
    }
}

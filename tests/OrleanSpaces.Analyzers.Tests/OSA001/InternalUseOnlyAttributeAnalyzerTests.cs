using OrleanSpaces.Analyzers.OSA001;

namespace OrleanSpaces.Analyzers.Tests.OSA001;

public class InternalUseOnlyAttributeAnalyzerTests : AnalyzerFixture
{
    public InternalUseOnlyAttributeAnalyzerTests() : base(
        new InternalUseOnlyAttributeAnalyzer(),
        InternalUseOnlyAttributeAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = InternalUseOnlyAttributeAnalyzer.Diagnostic;

        Assert.Equal("OSA001", diagnostic.Id);
        Assert.Equal(Categories.Usage, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Warning, diagnostic.DefaultSeverity);
        Assert.Equal("Interface is intended for internal use only.", diagnostic.Title);
        Assert.Equal("Interface '{0}' is intended for internal use only.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("[|ISpaceTuple|] tuple  = new SpaceTuple(1);")]
    [InlineData("[|ISpaceTemplate|] tuple = new SpaceTemplate(1);")]

    [InlineData("[|ISpaceTuple<int>|] tuple = new IntTuple(1);")]
    [InlineData("[|ISpaceTemplate<int>|] tuple = new IntTemplate(1);")]
    public void Should_Diagnose_On_Assignment(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);

    [Theory]
    [InlineData("class A { public B([|ISpaceTuple|] c) }")]
    [InlineData("class A { public B([|ISpaceTemplate|] c) }")]

    [InlineData("class A { public B([|ISpaceTuple<int>|] c) }")]
    [InlineData("class A { public B([|ISpaceTemplate<int>|] c) }")]
    public void Should_Diagnose_On_Argument_Passing(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);

    [Theory]
    [InlineData("class A : [|ISpaceTuple|] { public int Length => 0; }")]
    [InlineData("class A : [|ISpaceTemplate|] { public int Length => 0; }")]

    [InlineData(@"
class A : [|ISpaceTuple<int>|]
{
    private int value;
    public ref readonly int this[int index] => ref value;
    public int Length => 0;
    public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
    public ReadOnlySpan<int>.Enumerator GetEnumerator() => new();
}")]
    [InlineData(@"
class A : [|ISpaceTemplate<int>|]
{
    private int? value;
    public ref readonly int? this[int index] => ref value;
    public int Length => 0;
    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new();
}")]
    public void Should_Diagnose_On_Implementing(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
}

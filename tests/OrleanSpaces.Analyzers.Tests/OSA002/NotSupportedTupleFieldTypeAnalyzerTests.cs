using Microsoft.CodeAnalysis;
using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class NotSupportedTupleFieldTypeAnalyzerTests : AnalyzerFixture
{
    public NotSupportedTupleFieldTypeAnalyzerTests() : base(
        new NotSupportedTupleFieldTypeAnalyzer(),
        NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OSA002", NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id);
        Assert.Equal(Categories.Usage, NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Warning, NotSupportedTupleFieldTypeAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("The supplied argument is not a supported type.", NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Title);
        Assert.Equal("The supplied argument '{0}' is not a supported '{1}' type.", NotSupportedTupleFieldTypeAnalyzer.Diagnostic.MessageFormat);
        Assert.True(NotSupportedTupleFieldTypeAnalyzer.Diagnostic.IsEnabledByDefault);
    }

    [Theory]

    [InlineData("SpaceTuple tuple = [|new(typeof(string))|];")]
    [InlineData("SpaceTuple tuple = [|new(typeof(char))|];")]
    [InlineData("SpaceTuple tuple = [|new(typeof(int))|];")]
    [InlineData("SpaceTuple tuple = [|new(typeof(decimal))|];")]
    [InlineData("SpaceTuple tuple = [|new(new SpaceUnit())|];")]
    [InlineData("SpaceTuple tuple = [|new(SpaceUnit.Null)|];")]

    [InlineData("SpaceTuple tuple = [|new(new TestClass())|]; class TestClass {}")]
    [InlineData("SpaceTuple tuple = [|new(new TestStruct())|]; struct TestStruct {}")]
    public void Should_Diagnose_SpaceTuple(string code) =>
        HasDiagnostic(ComposeMarkup(code));

    [Theory]

    [InlineData("SpaceTuple tuple = [|new('a')|];")]
    [InlineData("SpaceTuple tuple = [|new(\"a\")|];")]

    [InlineData("SpaceTuple tuple = [|new(true)|]")]
    [InlineData("SpaceTuple tuple = [|new(false)|]")]

    [InlineData("SpaceTuple tuple = [|new((byte)1)|]")]
    [InlineData("SpaceTuple tuple = [|new((sbyte)1)|]")]

    [InlineData("SpaceTuple tuple = [|new((short)1)|]")]
    [InlineData("SpaceTuple tuple = [|new((ushort)1)|]")]

    [InlineData("SpaceTuple tuple = [|new((int)1)|]")]
    [InlineData("SpaceTuple tuple = [|new((uint)1)|]")]

    [InlineData("SpaceTuple tuple = [|new((long)1)|];")]
    [InlineData("SpaceTuple tuple = [|new((ulong)1)|];")]

    [InlineData("SpaceTuple tuple = [|new((float)1)|];")]
    [InlineData("SpaceTuple tuple = [|new((double)1)|];")]
    [InlineData("SpaceTuple tuple = [|new((decimal)1)|];")]
    

    [InlineData("SpaceTuple tuple = [|new(DateTime.MinValue)|];")]
    [InlineData("SpaceTuple tuple = [|new(DateTimeOffset.MinValue)|];")]
    [InlineData("SpaceTuple tuple = [|new(TimeSpan.Zero)|];")]
    [InlineData("SpaceTuple tuple = [|new(Guid.Empty)|];")]

    [InlineData("SpaceTuple tuple = [|new(TestEnum.A)|]; enum TestEnum { A }")]
    public void Should_Not_Diagnose_SpaceTuple(string code) =>
       NoDiagnostic(ComposeMarkup(code));

    //[Theory]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //[InlineData("")]
    //public void Should_Diagnose_SpaceTemplate(string code) =>
    //    HasDiagnostic(ComposeMarkup(code));

    //public void Should_Not_Diagnose_SpaceTemplate(string code) =>
    //  HasDiagnostic(ComposeMarkup(code));

    private static string ComposeMarkup(string code) => @$"using OrleanSpaces.Tuples;{code}";
}

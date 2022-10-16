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
    [InlineData("SpaceTuple tuple = new([|typeof(string)|]);")]
    [InlineData("SpaceTuple tuple = new([|typeof(char)|]);")]
    [InlineData("SpaceTuple tuple = new([|typeof(int)|]);")]
    [InlineData("SpaceTuple tuple = new([|typeof(decimal)|]);")]
    [InlineData("SpaceTuple tuple = new([|new SpaceUnit()|]);")]
    [InlineData("SpaceTuple tuple = new([|SpaceUnit.Null|]);")]

    [InlineData("SpaceTuple tuple = new([|new TestClass()|]); class TestClass {}")]
    [InlineData("SpaceTuple tuple = new([|new TestStruct()|]); struct TestStruct {}")]

    [InlineData("TestClass c = new(); SpaceTuple tuple = new([|c|]); class TestClass {}")]
    [InlineData("TestStruct s = new(); SpaceTuple tuple = new([|s|]); class TestStruct {}")]
    public void Should_Diagnose_SpaceTuple(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTuple tuple = new('a');")]
    [InlineData("SpaceTuple tuple = new(\"a\");")]
    [InlineData("SpaceTuple tuple = new(true);")]
    [InlineData("SpaceTuple tuple = new(false);")]
    [InlineData("SpaceTuple tuple = new((byte)1);")]
    [InlineData("SpaceTuple tuple = new((sbyte)1);")]
    [InlineData("SpaceTuple tuple = new((short)1);")]
    [InlineData("SpaceTuple tuple = new((ushort)1);")]
    [InlineData("SpaceTuple tuple = new((int)1);")]
    [InlineData("SpaceTuple tuple = new((uint)1);")]
    [InlineData("SpaceTuple tuple = new((long)1);")]
    [InlineData("SpaceTuple tuple = new((ulong)1);")]
    [InlineData("SpaceTuple tuple = new((float)1);")]
    [InlineData("SpaceTuple tuple = new((double)1);")]

    [InlineData("SpaceTuple tuple = new((decimal)1);")]
    [InlineData("SpaceTuple tuple = new(DateTime.MinValue);")]
    [InlineData("SpaceTuple tuple = new(DateTimeOffset.MinValue);")]
    [InlineData("SpaceTuple tuple = new(TimeSpan.MinValue);")]
    [InlineData("SpaceTuple tuple = new(Guid.Empty);")]

    [InlineData("SpaceTuple tuple = new(TestEnum.A); enum TestEnum { A }")]
    [InlineData("TestEnum e = TestEnum.A; SpaceTuple tuple = new(e); enum TestEnum { A }")]
    public void Should_Not_Diagnose_SpaceTuple(string code) =>
       NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTemplate template = new([|new TestClass()|]);class TestClass {}")]
    [InlineData("SpaceTemplate template = new([|new TestStruct()|]); struct TestStruct {}")]

    [InlineData("TestClass c = new(); SpaceTemplate template = new([|c|]); class TestClass {}")]
    [InlineData("TestStruct s = new(); SpaceTemplate template = new([|s|]); class TestStruct {}")]
    public void Should_Diagnose_SpaceTemplate(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTemplate template = new('a');")]
    [InlineData("SpaceTemplate template = new(\"a\");")]
    [InlineData("SpaceTemplate template = new(true);")]
    [InlineData("SpaceTemplate template = new(false);")]
    [InlineData("SpaceTemplate template = new((byte)1);")]
    [InlineData("SpaceTemplate template = new((sbyte)1);")]
    [InlineData("SpaceTemplate template = new((short)1);")]
    [InlineData("SpaceTemplate template = new((ushort)1);")]
    [InlineData("SpaceTemplate template = new((int)1);")]
    [InlineData("SpaceTemplate template = new((uint)1);")]
    [InlineData("SpaceTemplate template = new((long)1);")]
    [InlineData("SpaceTemplate template = new((ulong)1);")]
    [InlineData("SpaceTemplate template = new((float)1);")]
    [InlineData("SpaceTemplate template = new((double)1);")]
                        
    [InlineData("SpaceTemplate template = new((decimal)1);")]
    [InlineData("SpaceTemplate template = new(DateTime.MinValue);")]
    [InlineData("SpaceTemplate template = new(DateTimeOffset.MinValue);")]
    [InlineData("SpaceTemplate template = new(TimeSpan.MinValue);")]
    [InlineData("SpaceTemplate template = new(Guid.Empty);")]

    [InlineData("SpaceTemplate template = new(typeof(string));")]
    [InlineData("SpaceTemplate template = new(typeof(char));")]
    [InlineData("SpaceTemplate template = new(typeof(int));")]
    [InlineData("SpaceTemplate template = new(typeof(decimal));")]

    [InlineData("SpaceTemplate template = new(new SpaceUnit());")]
    [InlineData("SpaceTemplate template = new(SpaceUnit.Null);")]

    [InlineData("SpaceTemplate template = new(TestEnum.A); enum TestEnum { A }")]
    [InlineData("TestEnum e = TestEnum.A; SpaceTemplate template = new(e); enum TestEnum { A }")]
    public void Should_Not_Diagnose_SpaceTemplate(string code) =>
      NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);
}

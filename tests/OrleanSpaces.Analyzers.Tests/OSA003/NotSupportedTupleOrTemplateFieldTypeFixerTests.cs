using OrleanSpaces.Analyzers.OSA003;

namespace OrleanSpaces.Analyzers.Tests.OSA003;

public class NotSupportedTupleOrTemplateFieldTypeFixerTests : FixerFixture
{
    public NotSupportedTupleOrTemplateFieldTypeFixerTests() : base(
        new NotSupportedTupleOrTemplateFieldTypeAnalyzer(),
        new NotSupportedTupleOrTemplateFieldTypeFixer(),
        NotSupportedTupleOrTemplateFieldTypeAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal() =>
        Assert.Equal("OSA002", provider.FixableDiagnosticIds.Single());

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
    public void Should_Fix_All_Arguments_SpaceTuple(string code) =>
        TestCodeFix(code, RemoveTextWithinDiagnosticSpan(code), Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTuple tuple = new(1 [|, typeof(string)|], 'a');")]
    [InlineData("SpaceTuple tuple = new(1 [|, typeof(char)|], 'a');")]
    [InlineData("SpaceTuple tuple = new(1 [|, typeof(int)|], 'a');")]
    [InlineData("SpaceTuple tuple = new(1 [|, typeof(decimal)|], 'a');")]
    [InlineData("SpaceTuple tuple = new(1 [|, new SpaceUnit()|], 'a');")]
    [InlineData("SpaceTuple tuple = new(1 [|, SpaceUnit.Null|], 'a');")]

    [InlineData("SpaceTuple tuple = new(1 [|, new TestClass()|], 'a'); class TestClass {}")]
    [InlineData("SpaceTuple tuple = new(1 [|, new TestStruct()|], 'a'); struct TestStruct {}")]

    [InlineData("TestClass c = new(); SpaceTuple tuple = new(1 [|, c|], 'a'); class TestClass {}")]
    [InlineData("TestStruct s = new(); SpaceTuple tuple = new(1 [|, s|], 'a'); class TestStruct {}")]
    public void Should_Fix_Only_Unsupported_Arguments_SpaceTuple(string code) =>
        TestCodeFix(code, RemoveTextWithinDiagnosticSpan(code), Namespace.OrleanSpaces_Tuples);


    [Theory]
    [InlineData("SpaceTemplate template = new([|new TestClass()|]); class TestClass {}")]
    [InlineData("SpaceTemplate template = new([|new TestStruct()|]); struct TestStruct {}")]

    [InlineData("TestClass c = new(); SpaceTemplate template = new([|c|]); class TestClass {}")]
    [InlineData("TestStruct s = new(); SpaceTemplate template = new([|s|]); class TestStruct {}")]
    public void Should_Fix_All_Arguments_SpaceTemplate(string code) =>
        TestCodeFix(code, RemoveTextWithinDiagnosticSpan(code), Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("SpaceTemplate template = new(1 [|, new TestClass()|], 'a'); class TestClass {}")]
    [InlineData("SpaceTemplate template = new(1 [|, new TestStruct()|], 'a'); struct TestStruct {}")]

    [InlineData("TestClass c = new(); SpaceTemplate template = new(1 [|, c|], 'a'); class TestClass {}")]
    [InlineData("TestStruct s = new(); SpaceTemplate template = new(1 [|, s|], 'a'); class TestStruct {}")]
    public void Should_Fix_Only_Unsupported_Arguments_SpaceTemplate(string code) =>
        TestCodeFix(code, RemoveTextWithinDiagnosticSpan(code), Namespace.OrleanSpaces_Tuples);
}
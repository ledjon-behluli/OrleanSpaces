using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class NotSupportedTupleFieldTypeFixerTests : FixerFixture
{
	public NotSupportedTupleFieldTypeFixerTests() : base(
		new NotSupportedTupleFieldTypeAnalyzer(),
		new NotSupportedTupleFieldTypeFixer(),
		NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id)
	{

	}

	[Fact]
	public void Should_Equal()
	{
		Assert.Equal("OSA002", provider.FixableDiagnosticIds.Single());
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

    [InlineData("SpaceTuple tuple = new([|typeof(int), SpaceUnit.Null|]);")]
    [InlineData("SpaceTuple tuple = new([|typeof(int), typeof(string), SpaceUnit.Null|]);")]
    public void Should_Fix_All_Arguments_SpaceTuple(string code) =>
        TestCodeFix(code, RemoveTextWithinDiagnosticSpan(code), Namespace.OrleanSpaces_Tuples);

    // TODO: Fix me!
    [Fact]
    public void Should_Fix_Only_Unsupported_Arguments_SpaceTuple()
    {
        //const string code = "SpaceTuple tuple = new([|1, typeof(int), 'a', SpaceUnit.Null, 1.5f|]);";
        const string code = "SpaceTuple tuple = new(1, [|typeof(int)|], 'a', [|SpaceUnit.Null|], 1.5f);";
        const string fixedCode = "SpaceTuple tuple = new(1, 'a', 1.5f);";

        TestCodeFix(code, fixedCode, Namespace.OrleanSpaces_Tuples);
    }
}
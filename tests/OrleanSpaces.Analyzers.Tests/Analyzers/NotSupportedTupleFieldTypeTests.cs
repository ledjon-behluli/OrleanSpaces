

namespace OrleanSpaces.Analyzers.Tests.Analyzers;

//public class NotSupportedTupleFieldTypeAnalyzerTests : AnalyzerFixture
//{
//    public NotSupportedTupleFieldTypeAnalyzerTests() : base(
//        new NotSupportedTupleFieldTypeAnalyzer(), 
//        NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id)
//    {

//    }

//    private static string ComposeMarkup(string code) => @$"
//using OrleanSpaces.Tuples;

//class T 
//{{
//    void M() 
//    {{
//        {code}
//    }} 
//}}";

//    [Fact]
//    public void Test1()
//    {
//        HasDiagnostic(ComposeMarkup("SpaceTuple tuple = [|new(1)|];"));
//    }
//}

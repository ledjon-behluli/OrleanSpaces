using OrleanSpaces.Analyzers.OSA004;

namespace OrleanSpaces.Analyzers.Tests.OSA004;

public class PreferSpecializedOverGenericFixerTests : FixerFixture
{
    public PreferSpecializedOverGenericFixerTests() : base(
        new PreferSpecializedOverGenericAnalyzer(),
        new PreferSpecializedOverGenericFixer(),
        PreferSpecializedOverGenericAnalyzer.Diagnostic.Id)
    {
        
    }

    [Fact]
    public void Should_Equal() =>
      Assert.Equal("OSA004", provider.FixableDiagnosticIds.Single());

    [Theory]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(true);|]", "Bool")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((byte)1);|]", "Byte")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((sbyte)1);|]", "SByte")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple('a');|]", "Char")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((double)1);|]", "Double")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((float)1);|]", "Float")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((short)1);|]", "Short")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((ushort)1);|]", "UShort")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(1);|]", "Int")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((uint)1);|]", "UInt")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((long)1);|]", "Long")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((ulong)1);|]", "ULong")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((Int128)1);|]", "Huge")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((UInt128)1);|]", "UHuge")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple((decimal)1);|]", "Decimal")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(Guid.Empty);|]", "Guid")]
    public void Should_Fix_ObjInited_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData("[|SpaceTuple tuple = new(true);|]", "Bool")]
    [InlineData("[|SpaceTuple tuple = new((byte)1);|]", "Byte")]
    [InlineData("[|SpaceTuple tuple = new((sbyte)1);|]", "SByte")]
    [InlineData("[|SpaceTuple tuple = new('a');|]", "Char")]
    [InlineData("[|SpaceTuple tuple = new((double)1);|]", "Double")]
    [InlineData("[|SpaceTuple tuple = new((float)1);|]", "Float")]
    [InlineData("[|SpaceTuple tuple = new((short)1);|]", "Short")]
    [InlineData("[|SpaceTuple tuple = new((ushort)1);|]", "UShort")]
    [InlineData("[|SpaceTuple tuple = new(1);|]", "Int")]
    [InlineData("[|SpaceTuple tuple = new((uint)1);|]", "UInt")]
    [InlineData("[|SpaceTuple tuple = new((long)1);|]", "Long")]
    [InlineData("[|SpaceTuple tuple = new((ulong)1);|]", "ULong")]
    [InlineData("[|SpaceTuple tuple = new((Int128)1);|]", "Huge")]
    [InlineData("[|SpaceTuple tuple = new((UInt128)1);|]", "UHuge")]
    [InlineData("[|SpaceTuple tuple = new((decimal)1);|]", "Decimal")]
    [InlineData("[|SpaceTuple tuple = new(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|SpaceTuple tuple = new(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|SpaceTuple tuple = new(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|SpaceTuple tuple = new(Guid.Empty);|]", "Guid")]
    public void Should_Fix_SimplifiedObjInited_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData("[|var tuple = new SpaceTuple(true);|]", "Bool")]
    [InlineData("[|var tuple = new SpaceTuple((byte)1);|]", "Byte")]
    [InlineData("[|var tuple = new SpaceTuple((sbyte)1);|]", "SByte")]
    [InlineData("[|var tuple = new SpaceTuple('a');|]", "Char")]
    [InlineData("[|var tuple = new SpaceTuple((double)1);|]", "Double")]
    [InlineData("[|var tuple = new SpaceTuple((float)1);|]", "Float")]
    [InlineData("[|var tuple = new SpaceTuple((short)1);|]", "Short")]
    [InlineData("[|var tuple = new SpaceTuple((ushort)1);|]", "UShort")]
    [InlineData("[|var tuple = new SpaceTuple(1);|]", "Int")]
    [InlineData("[|var tuple = new SpaceTuple((uint)1);|]", "UInt")]
    [InlineData("[|var tuple = new SpaceTuple((long)1);|]", "Long")]
    [InlineData("[|var tuple = new SpaceTuple((ulong)1);|]", "ULong")]
    [InlineData("[|var tuple = new SpaceTuple((Int128)1);|]", "Huge")]
    [InlineData("[|var tuple = new SpaceTuple((UInt128)1);|]", "UHuge")]
    [InlineData("[|var tuple = new SpaceTuple((decimal)1);|]", "Decimal")]
    [InlineData("[|var tuple = new SpaceTuple(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|var tuple = new SpaceTuple(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|var tuple = new SpaceTuple(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|var tuple = new SpaceTuple(Guid.Empty);|]", "Guid")]
    public void Should_Fix_VarInited_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

//    [Theory]
//    [InlineData(
//@"class C
//{
//    public C()
//    {
//        [|SpaceTuple tuple = new(1);|]
//    }
//}"
//, "Int")]
    public void Should_Fix_SpaceTuple_1(string code, string specializedTypePrefix)
    {
        string fix = code.Replace("Space", specializedTypePrefix);
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }
}

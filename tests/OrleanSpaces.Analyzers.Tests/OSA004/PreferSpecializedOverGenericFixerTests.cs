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

    #region SpaceTuple

    #region ObjInit

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
    public void Should_Fix_GlobalStatement_ObjInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Fact]
    public void Should_Fix_LocalStatement_ObjInit_SpaceTuple()
    {
        string code =
@"class C
{
    public C()
    {
        [|SpaceTuple tuple = new SpaceTuple(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        IntTuple tuple = new IntTuple(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #region SimplifiedObjInit

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
    public void Should_Fix_GlobalStatement_SimplifiedObjInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }


    [Fact]
    public void Should_Fix_LocalStatement_SimplifiedObjInit_SpaceTuple()
    {
        string code =
@"class C
{
    public C()
    {
        [|SpaceTuple tuple = new(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        IntTuple tuple = new(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #region VarObjInit

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
    public void Should_Fix_GlobalStatement_VarObjInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Fact]
    public void Should_Fix_LocalStatement_VarObjInit_SpaceTuple()
    {
        string code =
@"class C
{
    public C()
    {
        [|var tuple = new SpaceTuple(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        var tuple = new IntTuple(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #endregion

    #region SpaceTemplate

    #region ObjInit

    [Theory]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(true);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(true, null);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(null, true);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((byte)1);|]", "Byte")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((sbyte)1);|]", "SByte")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate('a');|]", "Char")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((double)1);|]", "Double")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((float)1);|]", "Float")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((short)1);|]", "Short")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((ushort)1);|]", "UShort")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(1);|]", "Int")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((uint)1);|]", "UInt")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((long)1);|]", "Long")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((ulong)1);|]", "ULong")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((Int128)1);|]", "Huge")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((UInt128)1);|]", "UHuge")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate((decimal)1);|]", "Decimal")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|SpaceTemplate template = new SpaceTemplate(Guid.Empty);|]", "Guid")]
    public void Should_Fix_GlobalStatement_ObjInit_SpaceTemplate(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Fact]
    public void Should_Fix_LocalStatement_ObjInit_SpaceTemplate()
    {
        string code =
@"class C
{
    public C()
    {
        [|SpaceTemplate template = new SpaceTemplate(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        IntTemplate template = new IntTemplate(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #region SimplifiedObjInit

    [Theory]
    [InlineData("[|SpaceTemplate template = new(true);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new(true, null);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new(null, true);|]", "Bool")]
    [InlineData("[|SpaceTemplate template = new((byte)1);|]", "Byte")]
    [InlineData("[|SpaceTemplate template = new((sbyte)1);|]", "SByte")]
    [InlineData("[|SpaceTemplate template = new('a');|]", "Char")]
    [InlineData("[|SpaceTemplate template = new((double)1);|]", "Double")]
    [InlineData("[|SpaceTemplate template = new((float)1);|]", "Float")]
    [InlineData("[|SpaceTemplate template = new((short)1);|]", "Short")]
    [InlineData("[|SpaceTemplate template = new((ushort)1);|]", "UShort")]
    [InlineData("[|SpaceTemplate template = new(1);|]", "Int")]
    [InlineData("[|SpaceTemplate template = new((uint)1);|]", "UInt")]
    [InlineData("[|SpaceTemplate template = new((long)1);|]", "Long")]
    [InlineData("[|SpaceTemplate template = new((ulong)1);|]", "ULong")]
    [InlineData("[|SpaceTemplate template = new((Int128)1);|]", "Huge")]
    [InlineData("[|SpaceTemplate template = new((UInt128)1);|]", "UHuge")]
    [InlineData("[|SpaceTemplate template = new((decimal)1);|]", "Decimal")]
    [InlineData("[|SpaceTemplate template = new(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|SpaceTemplate template = new(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|SpaceTemplate template = new(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|SpaceTemplate template = new(Guid.Empty);|]", "Guid")]
    public void Should_Fix_GlobalStatement_SimplifiedObjInit_SpaceTemplate(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }


    [Fact]
    public void Should_Fix_LocalStatement_SimplifiedObjInit_SpaceTemplate()
    {
        string code =
@"class C
{
    public C()
    {
        [|SpaceTemplate template = new(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        IntTemplate template = new(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #region VarObjInit

    [Theory]
    [InlineData("[|var template = new SpaceTemplate(true);|]", "Bool")]
    [InlineData("[|var template = new SpaceTemplate(true, null);|]", "Bool")]
    [InlineData("[|var template = new SpaceTemplate(null, true);|]", "Bool")]
    [InlineData("[|var template = new SpaceTemplate((byte)1);|]", "Byte")]
    [InlineData("[|var template = new SpaceTemplate((sbyte)1);|]", "SByte")]
    [InlineData("[|var template = new SpaceTemplate('a');|]", "Char")]
    [InlineData("[|var template = new SpaceTemplate((double)1);|]", "Double")]
    [InlineData("[|var template = new SpaceTemplate((float)1);|]", "Float")]
    [InlineData("[|var template = new SpaceTemplate((short)1);|]", "Short")]
    [InlineData("[|var template = new SpaceTemplate((ushort)1);|]", "UShort")]
    [InlineData("[|var template = new SpaceTemplate(1);|]", "Int")]
    [InlineData("[|var template = new SpaceTemplate((uint)1);|]", "UInt")]
    [InlineData("[|var template = new SpaceTemplate((long)1);|]", "Long")]
    [InlineData("[|var template = new SpaceTemplate((ulong)1);|]", "ULong")]
    [InlineData("[|var template = new SpaceTemplate((Int128)1);|]", "Huge")]
    [InlineData("[|var template = new SpaceTemplate((UInt128)1);|]", "UHuge")]
    [InlineData("[|var template = new SpaceTemplate((decimal)1);|]", "Decimal")]
    [InlineData("[|var template = new SpaceTemplate(DateTime.MinValue);|]", "DateTime")]
    [InlineData("[|var template = new SpaceTemplate(DateTimeOffset.MinValue);|]", "DateTimeOffset")]
    [InlineData("[|var template = new SpaceTemplate(TimeSpan.MinValue);|]", "TimeSpan")]
    [InlineData("[|var template = new SpaceTemplate(Guid.Empty);|]", "Guid")]
    public void Should_Fix_GlobalStatement_VarObjInit_SpaceTemplate(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Fact]
    public void Should_Fix_LocalStatement_VarObjInit_SpaceTemplate()
    {
        string code =
@"class C
{
    public C()
    {
        [|var template = new SpaceTemplate(1);|]
    }
}";

        string fix =
@"class C
{
    public C()
    {
        var template = new IntTemplate(1);
    }
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    #endregion

    #endregion
}

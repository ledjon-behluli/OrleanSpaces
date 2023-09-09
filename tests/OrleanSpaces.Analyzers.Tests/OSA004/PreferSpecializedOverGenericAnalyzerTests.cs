using OrleanSpaces.Analyzers.OSA004;

namespace OrleanSpaces.Analyzers.Tests.OSA004;

public class PreferSpecializedOverGenericAnalyzerTests : AnalyzerFixture
{
    public PreferSpecializedOverGenericAnalyzerTests() : base(
        new PreferSpecializedOverGenericAnalyzer(),
        PreferSpecializedOverGenericAnalyzer.Diagnostic.Id)
    {
        
    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = PreferSpecializedOverGenericAnalyzer.Diagnostic;

        Assert.Equal("OSA004", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Prefer using specialized over generic type.", diagnostic.Title);
        Assert.Equal("Prefer using specialized '{0}' over generic '{1}' type.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("[|SpaceTuple tuple = new('a');|]")]
    [InlineData("[|SpaceTuple tuple = new(true);|]")]
    [InlineData("[|SpaceTuple tuple = new(false);|]")]
    [InlineData("[|SpaceTuple tuple = new((byte)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((sbyte)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((short)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((ushort)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((int)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((uint)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((long)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((ulong)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((float)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((double)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((decimal)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((Int128)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((UInt128)1);|]")]
    [InlineData("[|SpaceTuple tuple = new(DateTime.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(DateTimeOffset.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(TimeSpan.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(Guid.Empty);|]")]

    [InlineData("[|SpaceTuple tuple = new('a', 'b');|]")]
    [InlineData("[|SpaceTuple tuple = new(true, false);|]")]
    [InlineData("[|SpaceTuple tuple = new(false, true);|]")]
    [InlineData("[|SpaceTuple tuple = new((byte)1, byte)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((sbyte)1, (sbyte)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((short)1, (short)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((ushort)1, (ushort)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((int)1, (int)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((uint)1, (uint)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((long)1, (long)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((ulong)1, (ulong)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((float)1, (float)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((double)1, (double)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((decimal)1, (decimal)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((Int128)1, (Int128)2);|]")]
    [InlineData("[|SpaceTuple tuple = new((UInt128)1, (UInt128)2);|]")]
    [InlineData("[|SpaceTuple tuple = new(DateTime.MinValue, DateTime.MaxValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(TimeSpan.MinValue, TimeSpan.MaxValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(Guid.Empty, Guid.Empty);|]")]
    public void Should_Diagnose_SpaceTuple(string code) =>
       HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("[|SpaceTuple tuple = new();|]")]
    [InlineData("[|SpaceTuple tuple = new(1, 'a');|]")]
    [InlineData("[|SpaceTuple tuple = new(1, DateTime.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, DateTimeOffset.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, TimeSpan.MinValue);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, Guid.Empty);|]")]
           
    [InlineData("[|SpaceTuple tuple = new((int)1, (float)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((int)1, (long)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((int)1, (double)1);|]")]
    [InlineData("[|SpaceTuple tuple = new((int)1, (decimal)1);|]")]
             
    [InlineData("[|SpaceTuple tuple = new(1, 1f);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, 1l);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, 1d);|]")]
    [InlineData("[|SpaceTuple tuple = new(1, 1m);|]")]

    public void Should_Not_Diagnose_SpaceTuple(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("[|SpaceTemplate template = new('a');|]")]
    [InlineData("[|SpaceTemplate template = new(true);|]")]
    [InlineData("[|SpaceTemplate template = new(false);|]")]
    [InlineData("[|SpaceTemplate template = new((byte)1);|]")]
    [InlineData("[|SpaceTemplate template = new((sbyte)1);|]")]
    [InlineData("[|SpaceTemplate template = new((short)1);|]")]
    [InlineData("[|SpaceTemplate template = new((ushort)1);|]")]
    [InlineData("[|SpaceTemplate template = new((int)1);|]")]
    [InlineData("[|SpaceTemplate template = new((uint)1);|]")]
    [InlineData("[|SpaceTemplate template = new((long)1);|]")]
    [InlineData("[|SpaceTemplate template = new((ulong)1);|]")]
    [InlineData("[|SpaceTemplate template = new((float)1);|]")]
    [InlineData("[|SpaceTemplate template = new((double)1);|]")]
    [InlineData("[|SpaceTemplate template = new((decimal)1);|]")]
    [InlineData("[|SpaceTemplate template = new((Int128)1);|]")]
    [InlineData("[|SpaceTemplate template = new((UInt128)1);|]")]
    [InlineData("[|SpaceTemplate template = new(DateTime.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(DateTimeOffset.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(TimeSpan.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(Guid.Empty);|]")]

    [InlineData("[|SpaceTemplate template = new(1, null);|]")]
    [InlineData("[|SpaceTemplate template = new(null, 1);|]")]
    [InlineData("[|SpaceTemplate template = new('a', 'b');|]")]
    [InlineData("[|SpaceTemplate template = new(true, false);|]")]
    [InlineData("[|SpaceTemplate template = new(false, true);|]")]
    [InlineData("[|SpaceTemplate template = new((byte)1, byte)2);|]")]
    [InlineData("[|SpaceTemplate template = new((sbyte)1, (sbyte)2);|]")]
    [InlineData("[|SpaceTemplate template = new((short)1, (short)2);|]")]
    [InlineData("[|SpaceTemplate template = new((ushort)1, (ushort)2);|]")]
    [InlineData("[|SpaceTemplate template = new((int)1, (int)2);|]")]
    [InlineData("[|SpaceTemplate template = new((uint)1, (uint)2);|]")]
    [InlineData("[|SpaceTemplate template = new((long)1, (long)2);|]")]
    [InlineData("[|SpaceTemplate template = new((ulong)1, (ulong)2);|]")]
    [InlineData("[|SpaceTemplate template = new((float)1, (float)2);|]")]
    [InlineData("[|SpaceTemplate template = new((double)1, (double)2);|]")]
    [InlineData("[|SpaceTemplate template = new((decimal)1, (decimal)2);|]")]
    [InlineData("[|SpaceTemplate template = new((Int128)1, (Int128)2);|]")]
    [InlineData("[|SpaceTemplate template = new((UInt128)1, (UInt128)2);|]")]
    [InlineData("[|SpaceTemplate template = new(DateTime.MinValue, DateTime.MaxValue);|]")]
    [InlineData("[|SpaceTemplate template = new(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);|]")]
    [InlineData("[|SpaceTemplate template = new(TimeSpan.MinValue, TimeSpan.MaxValue);|]")]
    [InlineData("[|SpaceTemplate template = new(Guid.Empty, Guid.Empty);|]")]
    public void Should_Diagnose_SpaceTemplate(string code) =>
       HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("[|SpaceTemplate template = new();|]")]
    [InlineData("[|SpaceTemplate template = new(null);|]")]
    [InlineData("[|SpaceTemplate template = new(1, 'a');|]")]
    [InlineData("[|SpaceTemplate template = new(1, DateTime.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(1, DateTimeOffset.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(1, TimeSpan.MinValue);|]")]
    [InlineData("[|SpaceTemplate template = new(1, Guid.Empty);|]")]
                 
    [InlineData("[|SpaceTemplate template = new((int)1, (float)1);|]")]
    [InlineData("[|SpaceTemplate template = new((int)1, (long)1);|]")]
    [InlineData("[|SpaceTemplate template = new((int)1, (double)1);|]")]
    [InlineData("[|SpaceTemplate template = new((int)1, (decimal)1);|]")]
                 
    [InlineData("[|SpaceTemplate template = new(1, 1f);|]")]
    [InlineData("[|SpaceTemplate template = new(1, 1l);|]")]
    [InlineData("[|SpaceTemplate template = new(1, 1d);|]")]
    [InlineData("[|SpaceTemplate template = new(1, 1m);|]")]

    public void Should_Not_Diagnose_SpaceTemplate(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);
}

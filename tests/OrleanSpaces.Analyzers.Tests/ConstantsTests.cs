namespace OrleanSpaces.Analyzers.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OrleanSpaces.InternalUseOnlyAttribute", FullyQualifiedNames.InternalUseOnlyAttribute);

        Assert.Equal("OrleanSpaces.Tuples.SpaceTuple", FullyQualifiedNames.SpaceTuple);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTemplate", FullyQualifiedNames.SpaceTemplate);

        Assert.Equal("OrleanSpaces.Tuples.Specialized.BoolTuple", FullyQualifiedNames.BoolTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ByteTuple", FullyQualifiedNames.ByteTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.CharTuple", FullyQualifiedNames.CharTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DateTimeOffsetTuple", FullyQualifiedNames.DateTimeOffsetTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DateTimeTuple", FullyQualifiedNames.DateTimeTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DecimalTuple", FullyQualifiedNames.DecimalTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DoubleTuple", FullyQualifiedNames.DoubleTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.FloatTuple", FullyQualifiedNames.FloatTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.GuidTuple", FullyQualifiedNames.GuidTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.HugeTuple", FullyQualifiedNames.HugeTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.IntTuple", FullyQualifiedNames.IntTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.LongTuple", FullyQualifiedNames.LongTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.SByteTuple", FullyQualifiedNames.SByteTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ShortTuple", FullyQualifiedNames.ShortTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.TimeSpanTuple", FullyQualifiedNames.TimeSpanTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UHugeTuple", FullyQualifiedNames.UHugeTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UIntTuple", FullyQualifiedNames.UIntTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ULongTuple", FullyQualifiedNames.ULongTuple);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UShortTuple", FullyQualifiedNames.UShortTuple);
        
        Assert.Equal("OrleanSpaces.Tuples.Specialized.BoolTemplate", FullyQualifiedNames.BoolTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ByteTemplate", FullyQualifiedNames.ByteTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.CharTemplate", FullyQualifiedNames.CharTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DateTimeOffsetTemplate", FullyQualifiedNames.DateTimeOffsetTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DateTimeTemplate", FullyQualifiedNames.DateTimeTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DecimalTemplate", FullyQualifiedNames.DecimalTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.DoubleTemplate", FullyQualifiedNames.DoubleTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.FloatTemplate", FullyQualifiedNames.FloatTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.GuidTemplate", FullyQualifiedNames.GuidTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.HugeTemplate", FullyQualifiedNames.HugeTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.IntTemplate", FullyQualifiedNames.IntTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.LongTemplate", FullyQualifiedNames.LongTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.SByteTemplate", FullyQualifiedNames.SByteTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ShortTemplate", FullyQualifiedNames.ShortTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.TimeSpanTemplate", FullyQualifiedNames.TimeSpanTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UHugeTemplate", FullyQualifiedNames.UHugeTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UIntTemplate", FullyQualifiedNames.UIntTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.ULongTemplate", FullyQualifiedNames.ULongTemplate);
        Assert.Equal("OrleanSpaces.Tuples.Specialized.UShortTemplate", FullyQualifiedNames.UShortTemplate);
    }
}


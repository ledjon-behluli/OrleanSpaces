using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class DateTimeOffsetTupleTests
{
    private static readonly DateTimeOffset offset1 = new(2023, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset2 = new(2023, 1, 2, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset3 = new(2023, 1, 3, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset4 = new(2023, 1, 4, 0, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Should_Be_Created_On_Array()
    {
        DateTimeOffsetTuple tuple = new(offset1, offset2, offset3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(offset1, tuple[0]);
        Assert.Equal(offset2, tuple[1]);
        Assert.Equal(offset3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        DateTimeOffsetTuple tuple = default;
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        DateTimeOffsetTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DateTimeOffsetTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DateTimeOffsetTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DateTimeOffsetTuple tuple1 = new(offset1, offset2, offset3);
        DateTimeOffsetTuple tuple2 = new(offset1, offset2, offset3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DateTimeOffsetTuple tuple1 = new();
        DateTimeOffsetTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DateTimeOffsetTuple tuple = new(offset1, offset2, offset3);
        object obj = new DateTimeOffsetTuple(offset1, offset2, offset3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DateTimeOffsetTuple tuple = new(offset1, offset2, offset3);
        object obj = new DateTimeOffsetTuple(offset1, offset2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DateTimeOffsetTuple tuple1 = new(offset1, offset2, offset3);
        DateTimeOffsetTuple tuple2 = new(offset1, offset2, offset4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DateTimeOffsetTuple tuple1 = new(offset1, offset2, offset3);
        DateTimeOffsetTuple tuple2 = new(offset1, offset2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DateTimeOffsetTuple().ToString());
        Assert.Equal($"({offset1})", new DateTimeOffsetTuple(offset1).ToString());
        Assert.Equal($"({offset1}, {offset2})", new DateTimeOffsetTuple(offset1, offset2).ToString());
        Assert.Equal($"({offset1}, {offset2}, {offset3})", new DateTimeOffsetTuple(offset1, offset2, offset3).ToString());
        Assert.Equal($"({offset1}, {offset2}, {offset3}, {offset4})", new DateTimeOffsetTuple(offset1, offset2, offset3, offset4).ToString());
    }
}

public class DateTimeOffsetTemplateTests
{
    private static readonly DateTimeOffset offset1 = new(2023, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset2 = new(2023, 1, 2, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset3 = new(2023, 1, 3, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset4 = new(2023, 1, 4, 0, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Should_Be_Created_On_Array()
    {
        DateTimeOffsetTemplate template = new(offset1, offset2, offset3);

        Assert.Equal(3, template.Length);
        Assert.Equal(offset1, template[0]);
        Assert.Equal(offset2, template[1]);
        Assert.Equal(offset3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        DateTimeOffsetTemplate template = new(Array.Empty<DateTimeOffset?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        DateTimeOffsetTemplate template = default;
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        DateTimeOffsetTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DateTimeOffsetTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DateTimeOffsetTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new DateTimeOffsetTemplate(offset1, offset2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DateTimeOffsetTemplate template1 = new(offset1, offset2, offset3, null);
        DateTimeOffsetTemplate template2 = new(offset1, offset2, offset3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DateTimeOffsetTemplate template1 = new();
        DateTimeOffsetTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DateTimeOffsetTemplate template = new(offset1, offset2, offset3, null);
        object obj = new DateTimeOffsetTemplate(offset1, offset2, offset3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DateTimeOffsetTemplate template = new(offset1, offset2, offset3);
        object obj = new DateTimeOffsetTemplate(offset1, offset2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DateTimeOffsetTemplate template1 = new(offset1, offset2, offset3);
        DateTimeOffsetTemplate template2 = new(offset1, offset2, offset4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DateTimeOffsetTemplate template1 = new(offset1, offset2, offset3);
        DateTimeOffsetTemplate template2 = new(offset1, offset2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new DateTimeOffsetTemplate().ToString());
        Assert.Equal($"({offset1})", new DateTimeOffsetTemplate(offset1).ToString());
        Assert.Equal($"({offset1}, {offset2})", new DateTimeOffsetTemplate(offset1, offset2).ToString());
        Assert.Equal($"({offset1}, {offset2}, {offset3})", new DateTimeOffsetTemplate(offset1, offset2, offset3).ToString());
        Assert.Equal($"({offset1}, {offset2}, {offset3}, {offset4})", new DateTimeOffsetTemplate(offset1, offset2, offset3, offset4).ToString());
        Assert.Equal($"({offset1}, {offset2}, {offset3}, {offset4}, {{NULL}})", new DateTimeOffsetTemplate(offset1, offset2, offset3, offset4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_DateTimeOffsetTuple()
    {
        DateTimeOffsetTemplate template1 = new();
        ISpaceTemplate<DateTimeOffset> explicit1 = new DateTimeOffsetTuple().ToTemplate();

        DateTimeOffsetTemplate template2 = new(offset1);
        ISpaceTemplate<DateTimeOffset> explicit2 = new DateTimeOffsetTuple(offset1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        DateTimeOffset[] fields = new DateTimeOffset[3] { offset1, offset2, offset3 };

        DateTimeOffsetTuple tuple1 = new(fields);
        DateTimeOffsetTuple tuple2 = (DateTimeOffsetTuple)Create<DateTimeOffsetTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<DateTimeOffset> Create<T>(DateTimeOffset[] fields)
            where T : ISpaceFactory<DateTimeOffset, DateTimeOffsetTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        DateTimeOffsetTuple tuple = new(offset1, offset2, offset3);
        int i = 0;
        
        foreach (ref readonly DateTimeOffset field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(DateTimeOffsetTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new DateTimeOffsetTuple(offset1), $"({offset1})", $"({offset1})".ToString().Length },
            new object[] { new DateTimeOffsetTuple(offset1, offset2), $"({offset1}, {offset2})", $"({offset1}, {offset2})".Length },
            new object[] { new DateTimeOffsetTuple(offset1, offset2, offset3), $"({offset1}, {offset2}, {offset3})", $"({offset1}, {offset2}, {offset3})".Length }
       };
}

public class DateTimeOffsetMatchTests
{
    private static readonly DateTimeOffset offset1 = new(2023, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset2 = new(2023, 1, 2, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset3 = new(2023, 1, 3, 0, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset offset4 = new(2023, 1, 4, 0, 0, 0, 0, TimeSpan.Zero);

    private readonly DateTimeOffsetTuple tuple;

    public DateTimeOffsetMatchTests()
    {
        tuple = new(offset1, offset2, offset3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        DateTimeOffsetTemplate template = new(offset1, offset2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new DateTimeOffsetTemplate(offset1).Matches(new DateTimeOffsetTuple(offset1)));
        Assert.True(new DateTimeOffsetTemplate(offset1, offset2).Matches(new DateTimeOffsetTuple(offset1, offset2)));
        Assert.True(new DateTimeOffsetTemplate(offset1, offset2, offset3).Matches(new DateTimeOffsetTuple(offset1, offset2, offset3)));
        Assert.True(new DateTimeOffsetTemplate(offset1, offset2, offset3, null).Matches(new DateTimeOffsetTuple(offset1, offset2, offset3, offset4)));
        Assert.True(new DateTimeOffsetTemplate(offset1, null, offset3, null).Matches(new DateTimeOffsetTuple(offset1, offset2, offset3, offset4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new DateTimeOffsetTemplate(offset1, offset2).Matches(new DateTimeOffsetTuple(offset1)));
        Assert.False(new DateTimeOffsetTemplate(offset1, offset2, offset3).Matches(new DateTimeOffsetTuple(offset1, offset1, offset3)));
        Assert.False(new DateTimeOffsetTemplate(offset1, offset2, offset3, null).Matches(new DateTimeOffsetTuple(offset1, offset1, offset3, offset4)));
    }
}

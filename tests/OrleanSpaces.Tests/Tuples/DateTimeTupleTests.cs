using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class DateTimeTupleTests
{
    private static readonly DateTime dateTime1 = new(2023, 1, 1, 0, 0, 0, 0);
    private static readonly DateTime dateTime2 = new(2023, 1, 2, 0, 0, 0, 0);
    private static readonly DateTime dateTime3 = new(2023, 1, 3, 0, 0, 0, 0);
    private static readonly DateTime dateTime4 = new(2023, 1, 4, 0, 0, 0, 0);

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        DateTimeTuple tuple = new(dateTime1, dateTime2, dateTime3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(dateTime1, tuple[0]);
        Assert.Equal(dateTime2, tuple[1]);
        Assert.Equal(dateTime3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        DateTimeTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DateTimeTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DateTimeTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DateTimeTuple tuple1 = new(dateTime1, dateTime2, dateTime3);
        DateTimeTuple tuple2 = new(dateTime1, dateTime2, dateTime3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DateTimeTuple tuple1 = new();
        DateTimeTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DateTimeTuple tuple = new(dateTime1, dateTime2, dateTime3);
        object obj = new DateTimeTuple(dateTime1, dateTime2, dateTime3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DateTimeTuple tuple = new(dateTime1, dateTime2, dateTime3);
        object obj = new DateTimeTuple(dateTime1, dateTime2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DateTimeTuple tuple1 = new(dateTime1, dateTime2, dateTime3);
        DateTimeTuple tuple2 = new(dateTime1, dateTime2, dateTime4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DateTimeTuple tuple1 = new(dateTime1, dateTime2, dateTime3);
        DateTimeTuple tuple2 = new(dateTime1, dateTime2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DateTimeTuple().ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM)", new DateTimeTuple(dateTime1).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM)", new DateTimeTuple(dateTime1, dateTime2).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM)", new DateTimeTuple(dateTime1, dateTime2, dateTime3).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM, 1/4/2023 12:00:00 AM)", new DateTimeTuple(dateTime1, dateTime2, dateTime3, dateTime4).ToString());
    }
}

public class DateTimeTemplateTests
{
    private static readonly DateTime dateTime1 = new(2023, 1, 1, 0, 0, 0, 0);
    private static readonly DateTime dateTime2 = new(2023, 1, 2, 0, 0, 0, 0);
    private static readonly DateTime dateTime3 = new(2023, 1, 3, 0, 0, 0, 0);
    private static readonly DateTime dateTime4 = new(2023, 1, 4, 0, 0, 0, 0);

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        DateTimeTemplate template = new(dateTime1, dateTime2, dateTime3);

        Assert.Equal(3, template.Length);
        Assert.Equal(dateTime1, template[0]);
        Assert.Equal(dateTime2, template[1]);
        Assert.Equal(dateTime3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        DateTimeTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DateTimeTemplate template = new(null);
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DateTimeTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new DateTimeTemplate(dateTime1, dateTime2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DateTimeTemplate template1 = new(dateTime1, dateTime2, dateTime3, null);
        DateTimeTemplate template2 = new(dateTime1, dateTime2, dateTime3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DateTimeTemplate template1 = new();
        DateTimeTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DateTimeTemplate template = new(dateTime1, dateTime2, dateTime3, null);
        object obj = new DateTimeTemplate(dateTime1, dateTime2, dateTime3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DateTimeTemplate template = new(dateTime1, dateTime2, dateTime3);
        object obj = new DateTimeTemplate(dateTime1, dateTime2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DateTimeTemplate template1 = new(dateTime1, dateTime2, dateTime3);
        DateTimeTemplate template2 = new(dateTime1, dateTime2, dateTime4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DateTimeTemplate template1 = new(dateTime1, dateTime2, dateTime3);
        DateTimeTemplate template2 = new(dateTime1, dateTime2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DateTimeTemplate().ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM)", new DateTimeTemplate(dateTime1).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM)", new DateTimeTemplate(dateTime1, dateTime2).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM)", new DateTimeTemplate(dateTime1, dateTime2, dateTime3).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM, 1/4/2023 12:00:00 AM)", new DateTimeTemplate(dateTime1, dateTime2, dateTime3, dateTime4).ToString());
        Assert.Equal("(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM, 1/4/2023 12:00:00 AM, {NULL})", new DateTimeTemplate(dateTime1, dateTime2, dateTime3, dateTime4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_DateTimeTuple()
    {
        DateTimeTemplate template1 = new();
        ISpaceTemplate<DateTime> explicit1 = new DateTimeTuple().ToTemplate();

        DateTimeTemplate template2 = new(dateTime1);
        ISpaceTemplate<DateTime> explicit2 = new DateTimeTuple(dateTime1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        DateTime[] fields = new DateTime[3] { dateTime1, dateTime2, dateTime3 };

        DateTimeTuple tuple1 = new(fields);
        DateTimeTuple tuple2 = (DateTimeTuple)Create<DateTimeTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<DateTime> Create<T>(DateTime[] fields)
            where T : ISpaceFactory<DateTime, DateTimeTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        DateTimeTuple tuple = new(dateTime1, dateTime2, dateTime3);
        int i = 0;
        
        foreach (ref readonly DateTime field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(DateTimeTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new DateTimeTuple(dateTime1), "(1/1/2023 12:00:00 AM)", 22 },
            new object[] { new DateTimeTuple(dateTime1, dateTime2), "(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM)", 44 },
            new object[] { new DateTimeTuple(dateTime1, dateTime2, dateTime3), "(1/1/2023 12:00:00 AM, 1/2/2023 12:00:00 AM, 1/3/2023 12:00:00 AM)", 66 }
       };
}

public class DateTimeMatchTests
{
    private static readonly DateTime dateTime1 = new(2023, 1, 1, 0, 0, 0, 0);
    private static readonly DateTime dateTime2 = new(2023, 1, 2, 0, 0, 0, 0);
    private static readonly DateTime dateTime3 = new(2023, 1, 3, 0, 0, 0, 0);
    private static readonly DateTime dateTime4 = new(2023, 1, 4, 0, 0, 0, 0);

    private readonly DateTimeTuple tuple;

    public DateTimeMatchTests()
    {
        tuple = new(dateTime1, dateTime2, dateTime3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        DateTimeTemplate template = new(dateTime1, dateTime2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new DateTimeTemplate(dateTime1).Matches(new DateTimeTuple(dateTime1)));
        Assert.True(new DateTimeTemplate(dateTime1, dateTime2).Matches(new DateTimeTuple(dateTime1, dateTime2)));
        Assert.True(new DateTimeTemplate(dateTime1, dateTime2, dateTime3).Matches(new DateTimeTuple(dateTime1, dateTime2, dateTime3)));
        Assert.True(new DateTimeTemplate(dateTime1, dateTime2, dateTime3, null).Matches(new DateTimeTuple(dateTime1, dateTime2, dateTime3, dateTime4)));
        Assert.True(new DateTimeTemplate(dateTime1, null, dateTime3, null).Matches(new DateTimeTuple(dateTime1, dateTime2, dateTime3, dateTime4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new DateTimeTemplate(dateTime1, dateTime2).Matches(new DateTimeTuple(dateTime1)));
        Assert.False(new DateTimeTemplate(dateTime1, dateTime2, dateTime3).Matches(new DateTimeTuple(dateTime1, dateTime1, dateTime3)));
        Assert.False(new DateTimeTemplate(dateTime1, dateTime2, dateTime3, null).Matches(new DateTimeTuple(dateTime1, dateTime1, dateTime3, dateTime4)));
    }
}

using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanBools.Tests.Tuples;

public class TimeSpanTupleTests
{
    private static readonly TimeSpan timeSpan1 = new(1, 0, 0);
    private static readonly TimeSpan timeSpan2 = new(2, 0, 0);
    private static readonly TimeSpan timeSpan3 = new(3, 0, 0);
    private static readonly TimeSpan timeSpan4 = new(4, 0, 0);

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        TimeSpanTuple tuple = new(timeSpan1, timeSpan2, timeSpan3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(timeSpan1, tuple[0]);
        Assert.Equal(timeSpan2, tuple[1]);
        Assert.Equal(timeSpan3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        TimeSpanTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new TimeSpanTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        TimeSpanTuple tuple1 = new(timeSpan1, timeSpan2, timeSpan3);
        TimeSpanTuple tuple2 = new(timeSpan1, timeSpan2, timeSpan3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        TimeSpanTuple tuple1 = new();
        TimeSpanTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        TimeSpanTuple tuple = new(timeSpan1, timeSpan2, timeSpan3);
        object obj = new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {    
        TimeSpanTuple tuple = new(timeSpan1, timeSpan2, timeSpan3);
        object obj = new TimeSpanTuple(timeSpan1, timeSpan2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        TimeSpanTuple tuple1 = new(timeSpan1, timeSpan2, timeSpan3);
        TimeSpanTuple tuple2 = new(timeSpan1, timeSpan2, timeSpan4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        TimeSpanTuple tuple1 = new(timeSpan1, timeSpan2, timeSpan3);
        TimeSpanTuple tuple2 = new(timeSpan1, timeSpan2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new TimeSpanTuple().ToString());
        Assert.Equal("(01:00:00)", new TimeSpanTuple(timeSpan1).ToString());
        Assert.Equal("(01:00:00, 02:00:00)", new TimeSpanTuple(timeSpan1, timeSpan2).ToString());
        Assert.Equal("(01:00:00, 02:00:00, 03:00:00)", new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3).ToString());
        Assert.Equal("(01:00:00, 02:00:00, 03:00:00, 04:00:00)", new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3, timeSpan4).ToString());
    }
}

public class TimeSpanTemplateTests
{
    private static readonly TimeSpan timeSpan1 = new(1, 0, 0);
    private static readonly TimeSpan timeSpan2 = new(2, 0, 0);
    private static readonly TimeSpan timeSpan3 = new(3, 0, 0);
    private static readonly TimeSpan timeSpan4 = new(4, 0, 0);

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        TimeSpanTemplate template = new(timeSpan1, timeSpan2, timeSpan3);

        Assert.Equal(3, template.Length);
        Assert.Equal(timeSpan1, template[0]);
        Assert.Equal(timeSpan2, template[1]);
        Assert.Equal(timeSpan3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        TimeSpanTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        TimeSpanTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new TimeSpanTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new TimeSpanTemplate(timeSpan1, timeSpan2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        TimeSpanTemplate template1 = new(timeSpan1, timeSpan2, timeSpan3, null);
        TimeSpanTemplate template2 = new(timeSpan1, timeSpan2, timeSpan3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        TimeSpanTemplate template1 = new();
        TimeSpanTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        TimeSpanTemplate template = new(timeSpan1, timeSpan2, timeSpan3, null);
        object obj = new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        TimeSpanTemplate template = new(timeSpan1, timeSpan2, timeSpan3);
        object obj = new TimeSpanTemplate(timeSpan1, timeSpan2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        TimeSpanTemplate template1 = new(timeSpan1, timeSpan2, timeSpan3);
        TimeSpanTemplate template2 = new(timeSpan1, timeSpan2, timeSpan4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        TimeSpanTemplate template1 = new(timeSpan1, timeSpan2, timeSpan3);
        TimeSpanTemplate template2 = new(timeSpan1, timeSpan2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new TimeSpanTemplate().ToString());
        Assert.Equal("(01:00:00)", new TimeSpanTemplate(timeSpan1).ToString());
        Assert.Equal("(01:00:00, 02:00:00)", new TimeSpanTemplate(timeSpan1, timeSpan2).ToString());
        Assert.Equal("(01:00:00, 02:00:00, 03:00:00)", new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3).ToString());
        Assert.Equal("(01:00:00, 02:00:00, 03:00:00, 04:00:00)", new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3, timeSpan4).ToString());
        Assert.Equal("(01:00:00, 02:00:00, 03:00:00, 04:00:00, {NULL})", new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3, timeSpan4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_TimeSpanTuple()
    {
        TimeSpanTemplate template1 = new();
        ISpaceTemplate<TimeSpan> explicit1 = new TimeSpanTuple().ToTemplate();

        TimeSpanTemplate template2 = new(timeSpan1);
        ISpaceTemplate<TimeSpan> explicit2 = new TimeSpanTuple(timeSpan1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        TimeSpan[] fields = new TimeSpan[3] { timeSpan1, timeSpan2, timeSpan3 };

        TimeSpanTuple tuple1 = new(fields);
        TimeSpanTuple tuple2 = (TimeSpanTuple)Create<TimeSpanTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<TimeSpan> Create<T>(TimeSpan[] fields)
            where T : ISpaceFactory<TimeSpan, TimeSpanTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        TimeSpanTuple tuple = new(timeSpan1, timeSpan2, timeSpan3);
        int i = 0;
        
        foreach (ref readonly TimeSpan field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(TimeSpanTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new TimeSpanTuple(timeSpan1), "(01:00:00)", 10 },
            new object[] { new TimeSpanTuple(timeSpan1, timeSpan2), "(01:00:00, 02:00:00)", 20 },
            new object[] { new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3), "(01:00:00, 02:00:00, 03:00:00)", 30 }
       };
}

public class TimeSpanMatchTests
{
    private static readonly TimeSpan timeSpan1 = new(1, 0, 0);
    private static readonly TimeSpan timeSpan2 = new(2, 0, 0);
    private static readonly TimeSpan timeSpan3 = new(3, 0, 0);
    private static readonly TimeSpan timeSpan4 = new(4, 0, 0);

    private readonly TimeSpanTuple tuple;

    public TimeSpanMatchTests()
    {
        tuple = new(timeSpan1, timeSpan2, timeSpan3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        TimeSpanTemplate template = new(timeSpan1, timeSpan2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new TimeSpanTemplate(timeSpan1).Matches(new TimeSpanTuple(timeSpan1)));
        Assert.True(new TimeSpanTemplate(timeSpan1, timeSpan2).Matches(new TimeSpanTuple(timeSpan1, timeSpan2)));
        Assert.True(new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3).Matches(new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3)));
        Assert.True(new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3, null).Matches(new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3, timeSpan4)));
        Assert.True(new TimeSpanTemplate(timeSpan1, null, timeSpan3, null).Matches(new TimeSpanTuple(timeSpan1, timeSpan2, timeSpan3, timeSpan4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new TimeSpanTemplate(timeSpan1, timeSpan2).Matches(new TimeSpanTuple(timeSpan1)));
        Assert.False(new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3).Matches(new TimeSpanTuple(timeSpan1, timeSpan1, timeSpan3)));
        Assert.False(new TimeSpanTemplate(timeSpan1, timeSpan2, timeSpan3, null).Matches(new TimeSpanTuple(timeSpan1, timeSpan1, timeSpan3, timeSpan4)));
    }
}

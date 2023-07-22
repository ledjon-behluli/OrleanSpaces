using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class LongTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        LongTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        LongTuple tuple = default;
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        LongTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        LongTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new LongTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        LongTuple tuple1 = new(1, 2, 3);
        LongTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        LongTuple tuple1 = new();
        LongTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        LongTuple tuple = new(1, 2, 3);
        object obj = new LongTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        LongTuple tuple = new(1, 2, 3);
        object obj = new LongTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        LongTuple tuple1 = new(1, 2, 3);
        LongTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        LongTuple tuple1 = new(1, 2, 3);
        LongTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new LongTuple().ToString());
        Assert.Equal("(1)", new LongTuple(1).ToString());
        Assert.Equal("(1, 2)", new LongTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new LongTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new LongTuple(1, 2, 3, 4).ToString());
    }
}

public class LongTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        LongTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        LongTemplate template = new(Array.Empty<long?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        LongTemplate template = default;
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        LongTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        LongTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new LongTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new LongTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        LongTemplate template1 = new(1, 2, 3, null);
        LongTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        LongTemplate template1 = new();
        LongTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        LongTemplate template = new(1, 2, 3, null);
        object obj = new LongTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        LongTemplate template = new(1, 2, 3);
        object obj = new LongTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        LongTemplate template1 = new(1, 2, 3);
        LongTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        LongTemplate template1 = new(1, 2, 3);
        LongTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new LongTemplate().ToString());
        Assert.Equal("(1)", new LongTemplate(1).ToString());
        Assert.Equal("(1, 2)", new LongTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new LongTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new LongTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new LongTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_LongTuple()
    {
        LongTemplate template1 = new();
        ISpaceTemplate<long> explicit1 = new LongTuple().ToTemplate();

        LongTemplate template2 = new(1);
        ISpaceTemplate<long> explicit2 = new LongTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        long[] fields = new long[3] { 1, 2, 3 };

        LongTuple tuple1 = new(fields);
        LongTuple tuple2 = (LongTuple)Create<LongTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<long> Create<T>(long[] fields)
            where T : ISpaceFactory<long, LongTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        LongTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly long field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(LongTuple tuple, string spanString, long spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new LongTuple(1), "(1)", 3 },
            new object[] { new LongTuple(1, 2), "(1, 2)", 6 },
            new object[] { new LongTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class LongMatchTests
{
    private readonly LongTuple tuple;

    public LongMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        LongTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new LongTemplate(1).Matches(new LongTuple(1)));
        Assert.True(new LongTemplate(1, 2).Matches(new LongTuple(1, 2)));
        Assert.True(new LongTemplate(1, 2, 3).Matches(new LongTuple(1, 2, 3)));
        Assert.True(new LongTemplate(1, 2, 3, null).Matches(new LongTuple(1, 2, 3, 4)));
        Assert.True(new LongTemplate(1, null, 3, null).Matches(new LongTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new LongTemplate(1, 2).Matches(new LongTuple(1)));
        Assert.False(new LongTemplate(1, 2, 3).Matches(new LongTuple(1, 1, 3)));
        Assert.False(new LongTemplate(1, 2, 3, null).Matches(new LongTuple(1, 1, 3, 4)));
    }
}

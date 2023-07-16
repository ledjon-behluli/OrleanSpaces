using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class ULongTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        ULongTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((ulong)1, tuple[0]);
        Assert.Equal((ulong)2, tuple[1]);
        Assert.Equal((ulong)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        ULongTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        ULongTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ULongTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ULongTuple tuple1 = new(1, 2, 3);
        ULongTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ULongTuple tuple1 = new();
        ULongTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ULongTuple tuple = new(1, 2, 3);
        object obj = new ULongTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ULongTuple tuple = new(1, 2, 3);
        object obj = new ULongTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ULongTuple tuple1 = new(1, 2, 3);
        ULongTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ULongTuple tuple1 = new(1, 2, 3);
        ULongTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new ULongTuple().ToString());
        Assert.Equal("(1)", new ULongTuple(1).ToString());
        Assert.Equal("(1, 2)", new ULongTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ULongTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ULongTuple(1, 2, 3, 4).ToString());
    }
}

public class ULongTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        ULongTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((ulong)1, template[0]);
        Assert.Equal((ulong)2, template[1]);
        Assert.Equal((ulong)3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        ULongTemplate template = new(Array.Empty<ulong?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        ULongTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        ULongTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ULongTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new ULongTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ULongTemplate template1 = new(1, 2, 3, null);
        ULongTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ULongTemplate template1 = new();
        ULongTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ULongTemplate template = new(1, 2, 3, null);
        object obj = new ULongTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ULongTemplate template = new(1, 2, 3);
        object obj = new ULongTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ULongTemplate template1 = new(1, 2, 3);
        ULongTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ULongTemplate template1 = new(1, 2, 3);
        ULongTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new ULongTemplate().ToString());
        Assert.Equal("(1)", new ULongTemplate(1).ToString());
        Assert.Equal("(1, 2)", new ULongTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ULongTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ULongTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new ULongTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_ULongTuple()
    {
        ULongTemplate template1 = new();
        ISpaceTemplate<ulong> explicit1 = new ULongTuple().ToTemplate();

        ULongTemplate template2 = new(1);
        ISpaceTemplate<ulong> explicit2 = new ULongTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        ulong[] fields = new ulong[3] { 1, 2, 3 };

        ULongTuple tuple1 = new(fields);
        ULongTuple tuple2 = (ULongTuple)Create<ULongTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<ulong> Create<T>(ulong[] fields)
            where T : ISpaceFactory<ulong, ULongTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        ULongTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly ulong field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(ULongTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new ULongTuple(1), "(1)", 3 },
            new object[] { new ULongTuple(1, 2), "(1, 2)", 6 },
            new object[] { new ULongTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class ULongMatchTests
{
    private readonly ULongTuple tuple;

    public ULongMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        ULongTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new ULongTemplate(1).Matches(new ULongTuple(1)));
        Assert.True(new ULongTemplate(1, 2).Matches(new ULongTuple(1, 2)));
        Assert.True(new ULongTemplate(1, 2, 3).Matches(new ULongTuple(1, 2, 3)));
        Assert.True(new ULongTemplate(1, 2, 3, null).Matches(new ULongTuple(1, 2, 3, 4)));
        Assert.True(new ULongTemplate(1, null, 3, null).Matches(new ULongTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new ULongTemplate(1, 2).Matches(new ULongTuple(1)));
        Assert.False(new ULongTemplate(1, 2, 3).Matches(new ULongTuple(1, 1, 3)));
        Assert.False(new ULongTemplate(1, 2, 3, null).Matches(new ULongTuple(1, 1, 3, 4)));
    }
}

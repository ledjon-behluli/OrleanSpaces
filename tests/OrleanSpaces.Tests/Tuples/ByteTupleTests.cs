using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class ByteTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        ByteTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((byte)1, tuple[0]);
        Assert.Equal((byte)2, tuple[1]);
        Assert.Equal((byte)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        ByteTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ByteTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ByteTuple tuple1 = new(1, 2, 3);
        ByteTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ByteTuple tuple1 = new();
        ByteTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        ByteTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ByteTuple tuple = new(1, 2, 3);
        object obj = new ByteTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ByteTuple tuple = new(1, 2, 3);
        object obj = new ByteTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ByteTuple tuple1 = new(1, 2, 3);
        ByteTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ByteTuple tuple1 = new(1, 2, 3);
        ByteTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new ByteTuple().ToString());
        Assert.Equal("(1)", new ByteTuple(1).ToString());
        Assert.Equal("(1, 2)", new ByteTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ByteTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ByteTuple(1, 2, 3, 4).ToString());
    }
}

public class ByteTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        ByteTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((byte)1, template[0]);
        Assert.Equal((byte)2, template[1]);
        Assert.Equal((byte)3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        ByteTemplate template = new(Array.Empty<byte?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        ByteTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        ByteTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ByteTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new ByteTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ByteTemplate template1 = new(1, 2, 3, null);
        ByteTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ByteTemplate template1 = new();
        ByteTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ByteTemplate template = new(1, 2, 3, null);
        object obj = new ByteTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ByteTemplate template = new(1, 2, 3);
        object obj = new ByteTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ByteTemplate template1 = new(1, 2, 3);
        ByteTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ByteTemplate template1 = new(1, 2, 3);
        ByteTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new ByteTemplate().ToString());
        Assert.Equal("(1)", new ByteTemplate(1).ToString());
        Assert.Equal("(1, 2)", new ByteTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ByteTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ByteTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new ByteTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_ByteTuple()
    {
        ByteTemplate template1 = new();
        ISpaceTemplate<byte> explicit1 = new ByteTuple().ToTemplate();

        ByteTemplate template2 = new(1);
        ISpaceTemplate<byte> explicit2 = new ByteTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        byte[] fields = new byte[3] { 1, 2, 3 };

        ByteTuple tuple1 = new(fields);
        ByteTuple tuple2 = (ByteTuple)Create<ByteTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<byte> Create<T>(byte[] fields)
            where T : ISpaceFactory<byte, ByteTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        ByteTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly byte field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(ByteTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new ByteTuple(1), "(1)", 3 },
            new object[] { new ByteTuple(1, 2), "(1, 2)", 6 },
            new object[] { new ByteTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class ByteMatchTests
{
    private readonly ByteTuple tuple;

    public ByteMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        ByteTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new ByteTemplate(1).Matches(new ByteTuple(1)));
        Assert.True(new ByteTemplate(1, 2).Matches(new ByteTuple(1, 2)));
        Assert.True(new ByteTemplate(1, 2, 3).Matches(new ByteTuple(1, 2, 3)));
        Assert.True(new ByteTemplate(1, 2, 3, null).Matches(new ByteTuple(1, 2, 3, 4)));
        Assert.True(new ByteTemplate(1, null, 3, null).Matches(new ByteTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new ByteTemplate(1, 2).Matches(new ByteTuple(1)));
        Assert.False(new ByteTemplate(1, 2, 3).Matches(new ByteTuple(1, 1, 3)));
        Assert.False(new ByteTemplate(1, 2, 3, null).Matches(new ByteTuple(1, 1, 3, 4)));
    }
}

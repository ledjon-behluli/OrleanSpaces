using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class SByteTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        SByteTuple tuple = new(-1, -2, -3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((sbyte)-1, tuple[0]);
        Assert.Equal((sbyte)-2, tuple[1]);
        Assert.Equal((sbyte)-3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        SByteTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        SByteTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SByteTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SByteTuple tuple1 = new(-1, -2, -3);
        SByteTuple tuple2 = new(-1, -2, -3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        SByteTuple tuple1 = new();
        SByteTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SByteTuple tuple = new(-1, -2, -3);
        object obj = new SByteTuple(-1, -2, -3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SByteTuple tuple = new(-1, -2, -3);
        object obj = new SByteTuple(-1, -2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SByteTuple tuple1 = new(-1, -2, -3);
        SByteTuple tuple2 = new(-1, -2, -4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SByteTuple tuple1 = new(-1, -2, -3);
        SByteTuple tuple2 = new(-1, -2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new SByteTuple().ToString());
        Assert.Equal("(-1)", new SByteTuple(-1).ToString());
        Assert.Equal("(-1, -2)", new SByteTuple(-1, -2).ToString());
        Assert.Equal("(-1, -2, -3)", new SByteTuple(-1, -2, -3).ToString());
        Assert.Equal("(-1, -2, -3, -4)", new SByteTuple(-1, -2, -3, -4).ToString());
    }
}

public class SByteTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        SByteTemplate template = new(-1, -2, -3);

        Assert.Equal(3, template.Length);
        Assert.Equal((sbyte)-1, template[0]);
        Assert.Equal((sbyte)-2, template[1]);
        Assert.Equal((sbyte)-3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        SByteTemplate template = new(Array.Empty<sbyte?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        SByteTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        SByteTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SByteTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new SByteTemplate(-1, -2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SByteTemplate template1 = new(-1, -2, -3, null);
        SByteTemplate template2 = new(-1, -2, -3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        SByteTemplate template1 = new();
        SByteTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SByteTemplate template = new(-1, -2, -3, null);
        object obj = new SByteTemplate(-1, -2, -3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SByteTemplate template = new(-1, -2, -3);
        object obj = new SByteTemplate(-1, -2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SByteTemplate template1 = new(-1, -2, -3);
        SByteTemplate template2 = new(-1, -2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SByteTemplate template1 = new(-1, -2, -3);
        SByteTemplate template2 = new(-1, -2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new SByteTemplate().ToString());
        Assert.Equal("(-1)", new SByteTemplate(-1).ToString());
        Assert.Equal("(-1, -2)", new SByteTemplate(-1, -2).ToString());
        Assert.Equal("(-1, -2, -3)", new SByteTemplate(-1, -2, -3).ToString());
        Assert.Equal("(-1, -2, -3, -4)", new SByteTemplate(-1, -2, -3, -4).ToString());
        Assert.Equal("(-1, -2, -3, -4, {NULL})", new SByteTemplate(-1, -2, -3, -4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_SByteTuple()
    {
        SByteTemplate template1 = new();
        ISpaceTemplate<sbyte> explicit1 = new SByteTuple().ToTemplate();

        SByteTemplate template2 = new(1);
        ISpaceTemplate<sbyte> explicit2 = new SByteTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        sbyte[] fields = new sbyte[3] { -1, -2, -3 };

        SByteTuple tuple1 = new(fields);
        SByteTuple tuple2 = (SByteTuple)Create<SByteTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<sbyte> Create<T>(sbyte[] fields)
            where T : ISpaceFactory<sbyte, SByteTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        SByteTuple tuple = new(-1, -2, -3);
        int i = 0;
        
        foreach (ref readonly sbyte field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(SByteTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new SByteTuple(-1), "(-1)", 4 },
            new object[] { new SByteTuple(-1, -2), "(-1, -2)", 8 },
            new object[] { new SByteTuple(-1, -2, -10), "(-1, -2, -10)", 13 }
       };
}

public class SByteMatchTests
{
    private readonly SByteTuple tuple;

    public SByteMatchTests()
    {
        tuple = new(-1, -2, -3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SByteTemplate template = new(-1, -2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new SByteTemplate(-1).Matches(new SByteTuple(-1)));
        Assert.True(new SByteTemplate(-1, -2).Matches(new SByteTuple(-1, -2)));
        Assert.True(new SByteTemplate(-1, -2, -3).Matches(new SByteTuple(-1, -2, -3)));
        Assert.True(new SByteTemplate(-1, -2, -3, null).Matches(new SByteTuple(-1, -2, -3, -4)));
        Assert.True(new SByteTemplate(-1, null, -3, null).Matches(new SByteTuple(-1, -2, -3, -4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new SByteTemplate(-1, -2).Matches(new SByteTuple(-1)));
        Assert.False(new SByteTemplate(-1, -2, -3).Matches(new SByteTuple(-1, -1, 3)));
        Assert.False(new SByteTemplate(-1, -2, -3, null).Matches(new SByteTuple(-1, -1, -3, -4)));
    }
}

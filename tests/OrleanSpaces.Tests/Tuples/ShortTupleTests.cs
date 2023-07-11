using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class ShortTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        ShortTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((short)1, tuple[0]);
        Assert.Equal((short)2, tuple[1]);
        Assert.Equal((short)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        ShortTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ShortTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ShortTuple tuple1 = new(1, 2, 3);
        ShortTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ShortTuple tuple1 = new();
        ShortTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ShortTuple tuple = new(1, 2, 3);
        object obj = new ShortTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ShortTuple tuple = new(1, 2, 3);
        object obj = new ShortTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ShortTuple tuple1 = new(1, 2, 3);
        ShortTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ShortTuple tuple1 = new(1, 2, 3);
        ShortTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new ShortTuple().ToString());
        Assert.Equal("(1)", new ShortTuple(1).ToString());
        Assert.Equal("(1, 2)", new ShortTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ShortTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ShortTuple(1, 2, 3, 4).ToString());
    }
}

public class ShortTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        ShortTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((short)1, template[0]);
        Assert.Equal((short)2, template[1]);
        Assert.Equal((short)3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        ShortTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        ShortTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new ShortTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new ShortTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        ShortTemplate template1 = new(1, 2, 3, null);
        ShortTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        ShortTemplate template1 = new();
        ShortTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        ShortTemplate template = new(1, 2, 3, null);
        object obj = new ShortTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        ShortTemplate template = new(1, 2, 3);
        object obj = new ShortTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        ShortTemplate template1 = new(1, 2, 3);
        ShortTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        ShortTemplate template1 = new(1, 2, 3);
        ShortTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new ShortTemplate().ToString());
        Assert.Equal("(1)", new ShortTemplate(1).ToString());
        Assert.Equal("(1, 2)", new ShortTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new ShortTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new ShortTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new ShortTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_ShortTuple()
    {
        ShortTemplate template1 = new();
        ISpaceTemplate<short> explicit1 = new ShortTuple().ToTemplate();

        ShortTemplate template2 = new(1);
        ISpaceTemplate<short> explicit2 = new ShortTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        short[] fields = new short[3] { 1, 2, 3 };

        ShortTuple tuple1 = new(fields);
        ShortTuple tuple2 = (ShortTuple)Create<ShortTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<short> Create<T>(short[] fields)
            where T : ISpaceFactory<short, ShortTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        ShortTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly short field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(ShortTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new ShortTuple(1), "(1)", 3 },
            new object[] { new ShortTuple(1, 2), "(1, 2)", 6 },
            new object[] { new ShortTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class ShortMatchTests
{
    private readonly ShortTuple tuple;

    public ShortMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        ShortTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new ShortTemplate(1).Matches(new ShortTuple(1)));
        Assert.True(new ShortTemplate(1, 2).Matches(new ShortTuple(1, 2)));
        Assert.True(new ShortTemplate(1, 2, 3).Matches(new ShortTuple(1, 2, 3)));
        Assert.True(new ShortTemplate(1, 2, 3, null).Matches(new ShortTuple(1, 2, 3, 4)));
        Assert.True(new ShortTemplate(1, null, 3, null).Matches(new ShortTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new ShortTemplate(1, 2).Matches(new ShortTuple(1)));
        Assert.False(new ShortTemplate(1, 2, 3).Matches(new ShortTuple(1, 1, 3)));
        Assert.False(new ShortTemplate(1, 2, 3, null).Matches(new ShortTuple(1, 1, 3, 4)));
    }
}

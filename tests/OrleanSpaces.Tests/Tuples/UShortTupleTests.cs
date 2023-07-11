using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class UShortTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UShortTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((ushort)1, tuple[0]);
        Assert.Equal((ushort)2, tuple[1]);
        Assert.Equal((ushort)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        UShortTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        UShortTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UShortTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UShortTuple tuple1 = new(1, 2, 3);
        UShortTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UShortTuple tuple1 = new();
        UShortTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UShortTuple tuple = new(1, 2, 3);
        object obj = new UShortTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UShortTuple tuple = new(1, 2, 3);
        object obj = new UShortTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UShortTuple tuple1 = new(1, 2, 3);
        UShortTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UShortTuple tuple1 = new(1, 2, 3);
        UShortTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UShortTuple().ToString());
        Assert.Equal("(1)", new UShortTuple(1).ToString());
        Assert.Equal("(1, 2)", new UShortTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UShortTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UShortTuple(1, 2, 3, 4).ToString());
    }
}

public class UShortTemplateTests
{
    [Fact]
    public void Should_Be_An_IUShortTemplate()
    {
        Assert.True(typeof(ISpaceTemplate<ushort>).IsAssignableFrom(typeof(UShortTemplate)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UShortTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((ushort)1, template[0]);
        Assert.Equal((ushort)2, template[1]);
        Assert.Equal((ushort)3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        UShortTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        UShortTemplate template = new(null);
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UShortTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new UShortTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UShortTemplate template1 = new(1, 2, 3, null);
        UShortTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UShortTemplate template1 = new();
        UShortTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UShortTemplate template = new(1, 2, 3, null);
        object obj = new UShortTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UShortTemplate template = new(1, 2, 3);
        object obj = new UShortTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UShortTemplate template1 = new(1, 2, 3);
        UShortTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UShortTemplate template1 = new(1, 2, 3);
        UShortTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UShortTemplate().ToString());
        Assert.Equal("(1)", new UShortTemplate(1).ToString());
        Assert.Equal("(1, 2)", new UShortTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UShortTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UShortTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new UShortTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_UShortTuple()
    {
        UShortTemplate template1 = new();
        ISpaceTemplate<ushort> explicit1 = new UShortTuple().ToTemplate();

        UShortTemplate template2 = new(1);
        ISpaceTemplate<ushort> explicit2 = new UShortTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        ushort[] fields = new ushort[3] { 1, 2, 3 };

        UShortTuple tuple1 = new(fields);
        UShortTuple tuple2 = (UShortTuple)Create<UShortTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<ushort> Create<T>(ushort[] fields)
            where T : ISpaceFactory<ushort, UShortTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        UShortTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly ushort field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(UShortTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new UShortTuple(1), "(1)", 3 },
            new object[] { new UShortTuple(1, 2), "(1, 2)", 6 },
            new object[] { new UShortTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class UShortMatchTests
{
    private readonly UShortTuple tuple;

    public UShortMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        UShortTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new UShortTemplate(1).Matches(new UShortTuple(1)));
        Assert.True(new UShortTemplate(1, 2).Matches(new UShortTuple(1, 2)));
        Assert.True(new UShortTemplate(1, 2, 3).Matches(new UShortTuple(1, 2, 3)));
        Assert.True(new UShortTemplate(1, 2, 3, null).Matches(new UShortTuple(1, 2, 3, 4)));
        Assert.True(new UShortTemplate(1, null, 3, null).Matches(new UShortTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new UShortTemplate(1, 2).Matches(new UShortTuple(1)));
        Assert.False(new UShortTemplate(1, 2, 3).Matches(new UShortTuple(1, 1, 3)));
        Assert.False(new UShortTemplate(1, 2, 3, null).Matches(new UShortTuple(1, 1, 3, 4)));
    }
}

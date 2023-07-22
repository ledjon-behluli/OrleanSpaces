using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class DoubleTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        DoubleTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        DoubleTuple tuple = default;
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        DoubleTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DoubleTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DoubleTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DoubleTuple tuple1 = new(1, 2, 3);
        DoubleTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DoubleTuple tuple1 = new();
        DoubleTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DoubleTuple tuple = new(1, 2, 3);
        object obj = new DoubleTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DoubleTuple tuple = new(1, 2, 3);
        object obj = new DoubleTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DoubleTuple tuple1 = new(1, 2, 3);
        DoubleTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DoubleTuple tuple1 = new(1, 2, 3);
        DoubleTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DoubleTuple().ToString());
        Assert.Equal("(1)", new DoubleTuple(1).ToString());
        Assert.Equal("(1, 2)", new DoubleTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new DoubleTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new DoubleTuple(1, 2, 3, 4).ToString());
    }
}

public class DoubleTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        DoubleTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        DoubleTemplate template = new(Array.Empty<double?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Keyword()
    {
        DoubleTemplate template = default;
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        DoubleTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DoubleTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DoubleTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new DoubleTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DoubleTemplate template1 = new(1, 2, 3, null);
        DoubleTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DoubleTemplate template1 = new();
        DoubleTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DoubleTemplate template = new(1, 2, 3, null);
        object obj = new DoubleTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DoubleTemplate template = new(1, 2, 3);
        object obj = new DoubleTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DoubleTemplate template1 = new(1, 2, 3);
        DoubleTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DoubleTemplate template1 = new(1, 2, 3);
        DoubleTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new DoubleTemplate().ToString());
        Assert.Equal("(1)", new DoubleTemplate(1).ToString());
        Assert.Equal("(1, 2)", new DoubleTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new DoubleTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new DoubleTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new DoubleTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_DoubleTuple()
    {
        DoubleTemplate template1 = new();
        ISpaceTemplate<double> explicit1 = new DoubleTuple().ToTemplate();

        DoubleTemplate template2 = new(1);
        ISpaceTemplate<double> explicit2 = new DoubleTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        double[] fields = new double[3] { 1, 2, 3 };

        DoubleTuple tuple1 = new(fields);
        DoubleTuple tuple2 = (DoubleTuple)Create<DoubleTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<double> Create<T>(double[] fields)
            where T : ISpaceFactory<double, DoubleTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        DoubleTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly double field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(DoubleTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new DoubleTuple(1), "(1)", 3 },
            new object[] { new DoubleTuple(1, 2), "(1, 2)", 6 },
            new object[] { new DoubleTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class DoubleMatchTests
{
    private readonly DoubleTuple tuple;

    public DoubleMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        DoubleTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new DoubleTemplate(1).Matches(new DoubleTuple(1)));
        Assert.True(new DoubleTemplate(1, 2).Matches(new DoubleTuple(1, 2)));
        Assert.True(new DoubleTemplate(1, 2, 3).Matches(new DoubleTuple(1, 2, 3)));
        Assert.True(new DoubleTemplate(1, 2, 3, null).Matches(new DoubleTuple(1, 2, 3, 4)));
        Assert.True(new DoubleTemplate(1, null, 3, null).Matches(new DoubleTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new DoubleTemplate(1, 2).Matches(new DoubleTuple(1)));
        Assert.False(new DoubleTemplate(1, 2, 3).Matches(new DoubleTuple(1, 1, 3)));
        Assert.False(new DoubleTemplate(1, 2, 3, null).Matches(new DoubleTuple(1, 1, 3, 4)));
    }
}

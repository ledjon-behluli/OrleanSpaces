using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class DecimalTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        DecimalTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        DecimalTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DecimalTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DecimalTuple tuple1 = new(1, 2, 3);
        DecimalTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DecimalTuple tuple1 = new();
        DecimalTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DecimalTuple tuple = new(1, 2, 3);
        object obj = new DecimalTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DecimalTuple tuple = new(1, 2, 3);
        object obj = new DecimalTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DecimalTuple tuple1 = new(1, 2, 3);
        DecimalTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DecimalTuple tuple1 = new(1, 2, 3);
        DecimalTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DecimalTuple().ToString());
        Assert.Equal("(1)", new DecimalTuple(1).ToString());
        Assert.Equal("(1, 2)", new DecimalTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new DecimalTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new DecimalTuple(1, 2, 3, 4).ToString());
    }
}

public class DecimalTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        DecimalTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        DecimalTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        DecimalTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new DecimalTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new DecimalTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        DecimalTemplate template1 = new(1, 2, 3, null);
        DecimalTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        DecimalTemplate template1 = new();
        DecimalTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        DecimalTemplate template = new(1, 2, 3, null);
        object obj = new DecimalTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        DecimalTemplate template = new(1, 2, 3);
        object obj = new DecimalTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        DecimalTemplate template1 = new(1, 2, 3);
        DecimalTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        DecimalTemplate template1 = new(1, 2, 3);
        DecimalTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new DecimalTemplate().ToString());
        Assert.Equal("(1)", new DecimalTemplate(1).ToString());
        Assert.Equal("(1, 2)", new DecimalTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new DecimalTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new DecimalTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new DecimalTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_DecimalTuple()
    {
        DecimalTemplate template1 = new();
        ISpaceTemplate<decimal> explicit1 = new DecimalTuple().ToTemplate();

        DecimalTemplate template2 = new(1);
        ISpaceTemplate<decimal> explicit2 = new DecimalTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        decimal[] fields = new decimal[3] { 1, 2, 3 };

        DecimalTuple tuple1 = new(fields);
        DecimalTuple tuple2 = (DecimalTuple)Create<DecimalTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<decimal> Create<T>(decimal[] fields)
            where T : ISpaceFactory<decimal, DecimalTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        DecimalTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly decimal field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(DecimalTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new DecimalTuple(1), "(1)", 3 },
            new object[] { new DecimalTuple(1, 2), "(1, 2)", 6 },
            new object[] { new DecimalTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class DecimalMatchTests
{
    private readonly DecimalTuple tuple;

    public DecimalMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        DecimalTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new DecimalTemplate(1).Matches(new DecimalTuple(1)));
        Assert.True(new DecimalTemplate(1, 2).Matches(new DecimalTuple(1, 2)));
        Assert.True(new DecimalTemplate(1, 2, 3).Matches(new DecimalTuple(1, 2, 3)));
        Assert.True(new DecimalTemplate(1, 2, 3, null).Matches(new DecimalTuple(1, 2, 3, 4)));
        Assert.True(new DecimalTemplate(1, null, 3, null).Matches(new DecimalTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new DecimalTemplate(1, 2).Matches(new DecimalTuple(1)));
        Assert.False(new DecimalTemplate(1, 2, 3).Matches(new DecimalTuple(1, 1, 3)));
        Assert.False(new DecimalTemplate(1, 2, 3, null).Matches(new DecimalTuple(1, 1, 3, 4)));
    }
}

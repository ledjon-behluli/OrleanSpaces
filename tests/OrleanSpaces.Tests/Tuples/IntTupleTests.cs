using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class IntTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        IntTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        IntTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        IntTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new IntTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        IntTuple tuple1 = new(1, 2, 3);
        IntTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        IntTuple tuple1 = new();
        IntTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        IntTuple tuple = new(1, 2, 3);
        object obj = new IntTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        IntTuple tuple = new(1, 2, 3);
        object obj = new IntTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        IntTuple tuple1 = new(1, 2, 3);
        IntTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        IntTuple tuple1 = new(1, 2, 3);
        IntTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new IntTuple().ToString());
        Assert.Equal("(1)", new IntTuple(1).ToString());
        Assert.Equal("(1, 2)", new IntTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new IntTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new IntTuple(1, 2, 3, 4).ToString());
    }
}

public class IntTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        IntTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        IntTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        IntTemplate template = new(null);
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new IntTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new IntTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        IntTemplate template1 = new(1, 2, 3, null);
        IntTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        IntTemplate template1 = new();
        IntTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        IntTemplate template = new(1, 2, 3, null);
        object obj = new IntTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        IntTemplate template = new(1, 2, 3);
        object obj = new IntTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        IntTemplate template1 = new(1, 2, 3);
        IntTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        IntTemplate template1 = new(1, 2, 3);
        IntTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new IntTemplate().ToString());
        Assert.Equal("(1)", new IntTemplate(1).ToString());
        Assert.Equal("(1, 2)", new IntTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new IntTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new IntTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new IntTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_IntTuple()
    {
        IntTemplate template1 = new();
        ISpaceTemplate<int> explicit1 = new IntTuple().ToTemplate();

        IntTemplate template2 = new(1);
        ISpaceTemplate<int> explicit2 = new IntTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        int[] fields = new int[3] { 1, 2, 3 };

        IntTuple tuple1 = new(fields);
        IntTuple tuple2 = (IntTuple)Create<IntTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<int> Create<T>(int[] fields)
            where T : ISpaceFactory<int, IntTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        IntTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly int field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(IntTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new IntTuple(1), "(1)", 3 },
            new object[] { new IntTuple(1, 2), "(1, 2)", 6 },
            new object[] { new IntTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class IntMatchTests
{
    private readonly IntTuple tuple;

    public IntMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        IntTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new IntTemplate(1).Matches(new IntTuple(1)));
        Assert.True(new IntTemplate(1, 2).Matches(new IntTuple(1, 2)));
        Assert.True(new IntTemplate(1, 2, 3).Matches(new IntTuple(1, 2, 3)));
        Assert.True(new IntTemplate(1, 2, 3, null).Matches(new IntTuple(1, 2, 3, 4)));
        Assert.True(new IntTemplate(1, null, 3, null).Matches(new IntTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new IntTemplate(1, 2).Matches(new IntTuple(1)));
        Assert.False(new IntTemplate(1, 2, 3).Matches(new IntTuple(1, 1, 3)));
        Assert.False(new IntTemplate(1, 2, 3, null).Matches(new IntTuple(1, 1, 3, 4)));
    }
}

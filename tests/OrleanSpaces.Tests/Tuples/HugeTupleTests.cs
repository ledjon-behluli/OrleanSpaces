using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class HugeTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        HugeTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        HugeTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        HugeTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new HugeTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        HugeTuple tuple1 = new(1, 2, 3);
        HugeTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        HugeTuple tuple1 = new();
        HugeTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        HugeTuple tuple = new(1, 2, 3);
        object obj = new HugeTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        HugeTuple tuple = new(1, 2, 3);
        object obj = new HugeTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        HugeTuple tuple1 = new(1, 2, 3);
        HugeTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        HugeTuple tuple1 = new(1, 2, 3);
        HugeTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new HugeTuple().ToString());
        Assert.Equal("(1)", new HugeTuple(1).ToString());
        Assert.Equal("(1, 2)", new HugeTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new HugeTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new HugeTuple(1, 2, 3, 4).ToString());
    }
}

public class HugeTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        HugeTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        HugeTemplate template = new(Array.Empty<Int128?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        HugeTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        HugeTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new HugeTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new HugeTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        HugeTemplate template1 = new(1, 2, 3, null);
        HugeTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        HugeTemplate template1 = new();
        HugeTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        HugeTemplate template = new(1, 2, 3, null);
        object obj = new HugeTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        HugeTemplate template = new(1, 2, 3);
        object obj = new HugeTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        HugeTemplate template1 = new(1, 2, 3);
        HugeTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        HugeTemplate template1 = new(1, 2, 3);
        HugeTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new HugeTemplate().ToString());
        Assert.Equal("(1)", new HugeTemplate(1).ToString());
        Assert.Equal("(1, 2)", new HugeTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new HugeTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new HugeTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new HugeTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_HugeTuple()
    {
        HugeTemplate template1 = new();
        ISpaceTemplate<Int128> explicit1 = new HugeTuple().ToTemplate();

        HugeTemplate template2 = new(1);
        ISpaceTemplate<Int128> explicit2 = new HugeTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        Int128[] fields = new Int128[3] { 1, 2, 3 };

        HugeTuple tuple1 = new(fields);
        HugeTuple tuple2 = (HugeTuple)Create<HugeTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<Int128> Create<T>(Int128[] fields)
            where T : ISpaceFactory<Int128, HugeTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        HugeTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly Int128 field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(HugeTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new HugeTuple(1), "(1)", 3 },
            new object[] { new HugeTuple(1, 2), "(1, 2)", 6 },
            new object[] { new HugeTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class HugeMatchTests
{
    private readonly HugeTuple tuple;

    public HugeMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        HugeTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new HugeTemplate(1).Matches(new HugeTuple(1)));
        Assert.True(new HugeTemplate(1, 2).Matches(new HugeTuple(1, 2)));
        Assert.True(new HugeTemplate(1, 2, 3).Matches(new HugeTuple(1, 2, 3)));
        Assert.True(new HugeTemplate(1, 2, 3, null).Matches(new HugeTuple(1, 2, 3, 4)));
        Assert.True(new HugeTemplate(1, null, 3, null).Matches(new HugeTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new HugeTemplate(1, 2).Matches(new HugeTuple(1)));
        Assert.False(new HugeTemplate(1, 2, 3).Matches(new HugeTuple(1, 1, 3)));
        Assert.False(new HugeTemplate(1, 2, 3, null).Matches(new HugeTuple(1, 1, 3, 4)));
    }
}

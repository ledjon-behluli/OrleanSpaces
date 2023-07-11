using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class UHugeTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UHugeTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((UInt128)1, tuple[0]);
        Assert.Equal((UInt128)2, tuple[1]);
        Assert.Equal((UInt128)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        UHugeTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        UHugeTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UHugeTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UHugeTuple tuple1 = new(1, 2, 3);
        UHugeTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UHugeTuple tuple1 = new();
        UHugeTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UHugeTuple tuple = new(1, 2, 3);
        object obj = new UHugeTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UHugeTuple tuple = new(1, 2, 3);
        object obj = new UHugeTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UHugeTuple tuple1 = new(1, 2, 3);
        UHugeTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UHugeTuple tuple1 = new(1, 2, 3);
        UHugeTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UHugeTuple().ToString());
        Assert.Equal("(1)", new UHugeTuple(1).ToString());
        Assert.Equal("(1, 2)", new UHugeTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UHugeTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UHugeTuple(1, 2, 3, 4).ToString());
    }
}

public class UHugeTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UHugeTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((UInt128)1, template[0]);
        Assert.Equal((UInt128)2, template[1]);
        Assert.Equal((UInt128)3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        UHugeTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        UHugeTemplate template = new(null);
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UHugeTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new UHugeTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UHugeTemplate template1 = new(1, 2, 3, null);
        UHugeTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UHugeTemplate template1 = new();
        UHugeTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UHugeTemplate template = new(1, 2, 3, null);
        object obj = new UHugeTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UHugeTemplate template = new(1, 2, 3);
        object obj = new UHugeTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UHugeTemplate template1 = new(1, 2, 3);
        UHugeTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UHugeTemplate template1 = new(1, 2, 3);
        UHugeTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UHugeTemplate().ToString());
        Assert.Equal("(1)", new UHugeTemplate(1).ToString());
        Assert.Equal("(1, 2)", new UHugeTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UHugeTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UHugeTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new UHugeTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_UHugeTuple()
    {
        UHugeTemplate template1 = new();
        ISpaceTemplate<UInt128> explicit1 = new UHugeTuple().ToTemplate();

        UHugeTemplate template2 = new(1);
        ISpaceTemplate<UInt128> explicit2 = new UHugeTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        UInt128[] fields = new UInt128[3] { 1, 2, 3 };

        UHugeTuple tuple1 = new(fields);
        UHugeTuple tuple2 = (UHugeTuple)Create<UHugeTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<UInt128> Create<T>(UInt128[] fields)
            where T : ISpaceFactory<UInt128, UHugeTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        UHugeTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly UInt128 field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(UHugeTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new UHugeTuple(1), "(1)", 3 },
            new object[] { new UHugeTuple(1, 2), "(1, 2)", 6 },
            new object[] { new UHugeTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class UHugeMatchTests
{
    private readonly UHugeTuple tuple;

    public UHugeMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        UHugeTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new UHugeTemplate(1).Matches(new UHugeTuple(1)));
        Assert.True(new UHugeTemplate(1, 2).Matches(new UHugeTuple(1, 2)));
        Assert.True(new UHugeTemplate(1, 2, 3).Matches(new UHugeTuple(1, 2, 3)));
        Assert.True(new UHugeTemplate(1, 2, 3, null).Matches(new UHugeTuple(1, 2, 3, 4)));
        Assert.True(new UHugeTemplate(1, null, 3, null).Matches(new UHugeTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new UHugeTemplate(1, 2).Matches(new UHugeTuple(1)));
        Assert.False(new UHugeTemplate(1, 2, 3).Matches(new UHugeTuple(1, 1, 3)));
        Assert.False(new UHugeTemplate(1, 2, 3, null).Matches(new UHugeTuple(1, 1, 3, 4)));
    }
}
